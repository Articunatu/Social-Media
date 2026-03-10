using Microsoft.Extensions.Logging;
using SM.Application.Behaviors;

namespace SM.Infrastructure.Behaviors;

public class LoggingBehaviour(ILogger<LoggingBehaviour> logger) : ILoggingBehaviour
{
    public void LogCritical(string message, Exception? exception = null)
        => logger.LogCritical(exception, "{Message}", message);

    public void LogDebug(string message)
        => logger.LogDebug("{Message}", message);

    public void LogError(string message, Exception? exception = null)
        => logger.LogError(exception, "{Message}", message);

    public void LogInformation(string message)
        => logger.LogInformation("{Message}", message);

    public void LogTrace(string message)
        => logger.LogTrace("{Message}", message);

    public void LogWarning(string message)
        => logger.LogWarning("{Message}", message);
}
