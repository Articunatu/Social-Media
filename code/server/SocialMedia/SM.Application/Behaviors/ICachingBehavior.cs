using MediatR;
using SM.Domain.Shared;

namespace SM.Application.Behaviors;

public interface ICachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{ }
