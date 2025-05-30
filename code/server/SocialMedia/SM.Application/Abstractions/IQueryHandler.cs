using MediatR;
using SM.Domain.Shared;

namespace SM.Application.Abstractions;

public interface IQueryHandler<TQuery, TResponse>
: IRequestHandler<TQuery, Result<TResponse>>
where TQuery : IQuery<TResponse>
{ }