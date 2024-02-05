using MediatR;

namespace SocialMedia.Application.Behaviors
{
    public sealed class UnitOfWorkBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        public UnitOfWorkBehavior() { }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!typeof(TRequest).Name.EndsWith("Command"))
            {
                return await next();
            }

            var response = await next();

            return response;
        }

        private static bool IsNotCommand()
        {
            return !typeof(TRequest).Name.EndsWith("Command");
        }
    }
}