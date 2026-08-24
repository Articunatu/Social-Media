namespace SM.Domain.Messaging;

public sealed class ConversationParticipant(Guid conversationId, Guid userId)
{
    public Guid ConversationId { get; private set; } = conversationId;
    public Guid UserId { get; private set; } = userId;
    public Conversation Conversation { get; private set; } = default!;
}
