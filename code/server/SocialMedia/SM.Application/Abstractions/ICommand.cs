using MediatR;
using SM.Domain.Shared;

namespace SM.Application.Abstractions;

public interface ICommand : IRequest<Result> { }
public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }