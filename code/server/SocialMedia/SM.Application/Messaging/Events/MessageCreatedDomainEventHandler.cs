using MediatR;
using Microsoft.Extensions.Logging;
using SM.Domain.Messaging.Events;

namespace SM.Application.Messaging.Events;

internal sealed class MessageCreatedDomainEventHandler : INotificationHandler<MessageCreatedDomainEvent>
{
    private readonly ILogger<MessageCreatedDomainEventHandler> _logger;

    public MessageCreatedDomainEventHandler(ILogger<MessageCreatedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(MessageCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Message created: {MessageId}", notification.MessageId);
        return Task.CompletedTask;
    }
}
