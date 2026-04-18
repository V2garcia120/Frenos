using System.Diagnostics;
using System.Text;
using FrenosIntegracion.Data;
using FrenosIntegracion.Models.Entities;

namespace FrenosIntegracion.Middleware;

public class RequestLogMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _scopeFactory;

    public RequestLogMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
    {
        _next = next;
        _scopeFactory = scopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        // 1. Habilitar lectura múltiple del cuerpo de la petición
        context.Request.EnableBuffering();
        var requestBody = await LeerBodyAsync(context.Request.Body);
        context.Request.Body.Position = 0; // "Rebobinar" para que el Controller pueda leerlo

        // 2. Interceptar la respuesta (Buffer)
        var originalResponseBody = context.Response.Body;
        using var responseBuffer = new MemoryStream();
        context.Response.Body = responseBuffer;

        // Continuar con el siguiente paso (ir al Controller)
        await _next(context);

        stopwatch.Stop();

        // 3. Leer lo que el Controller respondió
        responseBuffer.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(responseBuffer).ReadToEndAsync();
        responseBuffer.Seek(0, SeekOrigin.Begin);

        // Copiar de vuelta al flujo original para que el cliente reciba la respuesta
        await responseBuffer.CopyToAsync(originalResponseBody);
        context.Response.Body = originalResponseBody;

        // 4. Guardar Log en segundo plano (para no poner lenta la App)
        _ = Task.Run(async () =>
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<IntegracionDbContext>();

            db.LogPeticiones.Add(new LogPeticion
            {
                Canal = context.Request.Headers["X-Canal"].ToString() ?? "Web",
                Metodo = context.Request.Method,
                Endpoint = context.Request.Path,
                RequestBody = requestBody,
                ResponseBody = responseBody,
                StatusCode = context.Response.StatusCode,
                DuracionMs = (int)stopwatch.ElapsedMilliseconds,
                FechaHora = DateTime.UtcNow
            });

            await db.SaveChangesAsync();
        });
    }

    private async Task<string> LeerBodyAsync(Stream body)
    {
        using var reader = new StreamReader(body, Encoding.UTF8, leaveOpen: true);
        var content = await reader.ReadToEndAsync();
        return content;
    }
}