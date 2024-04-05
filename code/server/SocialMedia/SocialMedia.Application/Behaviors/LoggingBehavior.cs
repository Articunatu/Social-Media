using MediatR;
using Serilog;
using SocialMedia.Application.Abstractions;

namespace SocialMedia.Application.Behaviors
{
    public class LoggingCommandBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
    {
        private readonly ILogger _logger;

        public LoggingCommandBehavior(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var name = request.GetType().Name;

            try
            {
                _logger.Information("Executing command {Command}", name);

                var result = await next();

                _logger.Information("Command {Command} processed successfully", name);

                return result;
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "Command {Command} processing failed", name);

                throw;
            }
        }
    }

    public class LoggingQueryBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
    {
        private readonly ILogger _logger;

        public LoggingQueryBehavior(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var name = request.GetType().Name;

            try
            {
                _logger.Information("Executing query {Query}", name);

                var result = await next();

                _logger.Information("Query {Query} processed successfully", name);

                return result;
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "Query {Query} processing failed", name);

                throw;
            }
        }
    }
}