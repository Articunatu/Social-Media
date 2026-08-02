using MediatR;
using Microsoft.Extensions.Logging;
using SM.Domain.Users.Events;

namespace SM.Application.Users.Events;

internal sealed class UserFollowedDomainEventHandler : INotificationHandler<UserFollowedDomainEvent>
{
    private readonly ILogger<UserFollowedDomainEventHandler> _logger;

    public UserFollowedDomainEventHandler(ILogger<UserFollowedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(UserFollowedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("User followed: {UserId}", notification.UserId);
        return Task.CompletedTask;
    }
}
