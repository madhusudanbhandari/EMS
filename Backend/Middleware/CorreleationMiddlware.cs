
namespace Backend.Middleware;

public class CorrelationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationMiddleware> _logger;

    public CorrelationMiddleware(RequestDelegate next, ILogger<CorrelationMiddleware> logger)
    {
        _next=next;
        _logger=logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId=context.Request.Headers["X-Correlation-ID"]
                        .FirstOrDefault();

        if (string.IsNullOrEmpty(correlationId))
        {
            correlationId=Guid.NewGuid().ToString();
        }

        context.Items["CorrelationId"]=correlationId;

        context.Response.Headers["X-Correlation-ID"]=correlationId;

        using(_logger.BeginScope(
            new Dictionary<string, object>
            {
                ["CorrelationId"]=correlationId
            }
        ))
        {
        await _next(context);
  
        }

    }
}