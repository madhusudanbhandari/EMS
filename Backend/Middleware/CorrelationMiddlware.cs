
using Microsoft.Net.Http.Headers;

namespace Backend.Middleware;

public class CorrelationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationMiddleware> _logger;

    private const string HeaderName="X-Correlation-ID";
    public CorrelationMiddleware(RequestDelegate next, ILogger<CorrelationMiddleware> logger)
    {
        _next=next;
        _logger=logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId=context.Request.Headers[HeaderName]
                        .FirstOrDefault();

        if (string.IsNullOrEmpty(correlationId))
        {
            correlationId=Guid.NewGuid().ToString();
        }

        context.Items["CorrelationId"]=correlationId;

        context.Response.Headers[HeaderName]=correlationId;

        using(_logger.BeginScope("CorrelationId:{CorrelationId}",correlationId))
        {
        await _next(context);
  
        }

    }
}