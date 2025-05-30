using MediatR;
using SM.Domain.Shared;

namespace SM.Application.Abstractions;

public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }