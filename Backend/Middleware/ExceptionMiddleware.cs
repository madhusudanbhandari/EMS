using System.Text.Json;

namespace Backend.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next=next;
        _logger=logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }catch(Exception ex)
        {
            var correlationId=context.Items["CorrelationId"]?.ToString();

            _logger.LogError(ex,
            "Unhandled exception occured while processing {Method} {Path} with CorrelationId {CorrelationId}",
            context.Request.Method,
            context.Request.Path,
            correlationId);


            context.Response.ContentType="application/json";
            context.Response.StatusCode=StatusCodes.Status500InternalServerError;

            var response =new
            {
                success=false,
                message=ex.Message,
                correlationId=correlationId
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response)
            );
        }
    }
}