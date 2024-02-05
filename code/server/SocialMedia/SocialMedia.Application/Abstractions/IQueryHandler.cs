using MediatR;
using SocialMedia.Domain.Shared;

namespace SocialMedia.Application.Abstractions
{
    public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
    { }
}