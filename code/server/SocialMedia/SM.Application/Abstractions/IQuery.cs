using MediatR;
using SocialMedia.Domain.Shared;

namespace SM.Application.Abstractions
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }
}