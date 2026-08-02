using MediatR;
using Microsoft.Extensions.Logging;
using SM.Domain.Content.Events;

namespace SM.Application.Content.Events;

internal sealed class PostCreatedDomainEventHandler : INotificationHandler<PostCreatedDomainEvent>
{
    private readonly ILogger<PostCreatedDomainEventHandler> _logger;

    public PostCreatedDomainEventHandler(ILogger<PostCreatedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(PostCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Post created: {PostId} by {AuthorId} at {CreatedAt}", notification.PostId, notification.AuthorId, notification.CreatedAt);
        return Task.CompletedTask;
    }
}
