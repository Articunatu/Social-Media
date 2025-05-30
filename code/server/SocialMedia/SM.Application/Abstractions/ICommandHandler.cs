using MediatR;
using SocialMedia.Domain.Shared;

namespace SM.Application.Abstractions
{
    public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result>
        where TCommand : ICommand
    { }

    public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
        where TCommand : ICommand<TResponse>
    { }
}