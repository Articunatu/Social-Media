using MediatR;
using Microsoft.Extensions.Logging;
using SM.Domain.Users.Events;

namespace SM.Application.Users.Events;

internal sealed class UserCreatedDomainEventHandler : INotificationHandler<UserCreatedDomainEvent>
{
    private readonly ILogger<UserCreatedDomainEventHandler> _logger;

    public UserCreatedDomainEventHandler(ILogger<UserCreatedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("User created: {UserId}", notification.UserId);
        return Task.CompletedTask;
    }
}
