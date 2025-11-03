namespace SM.Domain.Messages.DirectMessages;

public sealed class DirectMessage : Message
{
    public Guid ConversationId { get; private set; }
    public Conversation Conversation { get; private set; } = default!;

    public static DirectMessage Create(Guid conversationId, string content, Guid authorId)
    {
        return new DirectMessage
        {
            Id = Guid.CreateVersion7(),
            Content = content,
            TimeStamp = DateTimeOffset.UtcNow,
            AuthorId = authorId,
            ConversationId = conversationId
        };
    }
}
