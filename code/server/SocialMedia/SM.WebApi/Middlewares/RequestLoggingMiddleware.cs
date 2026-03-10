
namespace SM.WebApi.Middlewares;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger logger)
{
    public async Task Invoke(HttpContext context)
    {
        logger.LogInformation("Endpoint accessed: {Method} {Path} by {User} at {Time}",
            context.Request.Method,
            context.Request.Path,
            context.User.Identity?.Name ?? "Anonymous",
            DateTime.UtcNow);

        await next(context);
    }
}