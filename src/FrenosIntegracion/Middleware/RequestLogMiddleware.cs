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

        context.Request.EnableBuffering();
        var requestBody = await LeerBodyAsync(context.Request.Body);
        context.Request.Body.Position = 0;

        var originalResponseBody = context.Response.Body;
        using var responseBuffer = new MemoryStream();
        context.Response.Body = responseBuffer;

        await _next(context);

        stopwatch.Stop();

        responseBuffer.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(responseBuffer).ReadToEndAsync();
        responseBuffer.Seek(0, SeekOrigin.Begin);
        await responseBuffer.CopyToAsync(originalResponseBody);
        context.Response.Body = originalResponseBody;

        var canal = context.Request.Headers["X-Canal"].ToString() is { Length: > 0 } c ? c : "Web";
        var metodo = context.Request.Method;
        var endpoint = context.Request.Path.ToString();
        var statusCode = context.Response.StatusCode;
        var duracion = (int)stopwatch.ElapsedMilliseconds;

        _ = Task.Run(async () =>
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<IntegracionDbContext>();

            db.LogPeticiones.Add(new LogPeticion
            {
                Canal = canal,       
                Metodo = metodo,      
                Endpoint = endpoint,    
                RequestBody = requestBody, 
                ResponseBody = responseBody,
                StatusCode = statusCode,  
                DuracionMs = duracion,    
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