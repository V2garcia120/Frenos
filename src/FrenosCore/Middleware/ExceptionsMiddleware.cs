using FrenosCore.Helpers;
using System.Net;
using System.Text.Json;
using FrenosCore.Helpers;

namespace FrenosCore.Middleware;


public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Excepción no controlada: {Mensaje}", ex.Message);
            await ManejarExcepcionAsync(context, ex);
        }
    }

    private static async Task ManejarExcepcionAsync(HttpContext context, Exception ex)
    {

        var (statusCode, codigo) = ex switch
        {
            KeyNotFoundException => (HttpStatusCode.NotFound, "NOT_FOUND"),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "UNAUTHORIZED"),
            InvalidOperationException => (HttpStatusCode.UnprocessableEntity, "BUSINESS_RULE_ERROR"),
            ArgumentException => (HttpStatusCode.BadRequest, "VALIDATION_ERROR"),
            _ => (HttpStatusCode.InternalServerError, "INTERNAL_ERROR")
        };

        var mensaje = statusCode == HttpStatusCode.InternalServerError
            ? "Error interno del servidor. Contacte al administrador."
            : ex.Message;

        var respuesta = ApiResponse<object>.Fail(codigo, mensaje);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var json = JsonSerializer.Serialize(respuesta, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}