using MediatR;
using SocialMedia.Domain.Shared;

namespace SocialMedia.Application.Abstractions
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }
}