using Domain.Shared;
using MediatR;

namespace Application.Abstraction
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }
}
