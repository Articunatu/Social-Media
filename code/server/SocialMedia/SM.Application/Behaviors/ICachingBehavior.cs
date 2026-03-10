using MediatR;
using SM.Domain.Shared;

namespace SM.Application.Behaviors;

public interface ICachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, Result<TResponse>>
    where TRequest : IRequest<Result<TResponse>>
{ }
