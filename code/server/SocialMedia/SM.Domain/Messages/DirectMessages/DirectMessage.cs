
namespace SM.Domain.Messages.DirectMessages;

public sealed class DirectMessage(Guid id, string content, DateTime timeStamp, Guid authorId) 
    : Message(id, content, timeStamp, authorId)
{
    public Conversation Conversation { get; set; } = default!;
}
