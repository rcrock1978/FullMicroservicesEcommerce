namespace ApiGateway.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var requestId = Guid.NewGuid().ToString();
        context.Items["RequestId"] = requestId;

        _logger.LogInformation(
            "Request {RequestId}: {Method} {Path} from {RemoteIp}",
            requestId,
            context.Request.Method,
            context.Request.Path,
            context.Connection.RemoteIpAddress);

        var startTime = DateTime.UtcNow;

        try
        {
            await _next(context);

            var duration = DateTime.UtcNow - startTime;

            _logger.LogInformation(
                "Request {RequestId}: Completed with {StatusCode} in {Duration}ms",
                requestId,
                context.Response.StatusCode,
                duration.TotalMilliseconds);
        }
        catch (Exception ex)
        {
            var duration = DateTime.UtcNow - startTime;

            _logger.LogError(
                ex,
                "Request {RequestId}: Failed after {Duration}ms",
                requestId,
                duration.TotalMilliseconds);

            throw;
        }
    }
}
