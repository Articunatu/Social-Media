using SM.Domain.Abstractions;
using SM.Domain.Messaging.Events;

namespace SM.Domain.Messaging;

public sealed class DirectMessage(Guid id) : SoftDeletableEntity<Guid>(id)
{
    public string Content { get; set; } = string.Empty;
    public DateTimeOffset TimeStamp { get; set; }
    public Guid AuthorId { get; set; }
    public Guid ConversationId { get; private set; }
    public Conversation Conversation { get; private set; } = default!;

    public static DirectMessage Create(Guid conversationId, string content, Guid authorId)
    {
        var message = new DirectMessage(Guid.CreateVersion7())
        {
            Content = content,
            TimeStamp = DateTimeOffset.UtcNow,
            AuthorId = authorId,
            ConversationId = conversationId
        };

        message.RaiseDomainEvent(new MessageCreatedDomainEvent(
            message.Id,
            conversationId,
            authorId,
            message.TimeStamp));

        return message;
    }
}
