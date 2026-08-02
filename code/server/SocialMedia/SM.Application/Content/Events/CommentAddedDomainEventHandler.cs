using MediatR;
using Microsoft.Extensions.Logging;
using SM.Domain.Content.Events;

namespace SM.Application.Content.Events;

internal sealed class CommentAddedDomainEventHandler : INotificationHandler<CommentAddedDomainEvent>
{
    private readonly ILogger<CommentAddedDomainEventHandler> _logger;

    public CommentAddedDomainEventHandler(ILogger<CommentAddedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(CommentAddedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Comment added: {CommentId} on post {PostId} by {AuthorId} at {CreatedAt}", notification.CommentId, notification.PostId, notification.AuthorId, notification.CreatedAt);
        return Task.CompletedTask;
    }
}
