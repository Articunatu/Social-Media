using SM.Domain.Abstractions;

namespace SM.Domain.Messages;

public abstract class Message : Entity<Guid>, ISoftDeletable
{
    public Message() { }

    public Message(Guid id, string content, DateTime timeStamp) : base(id)
    {
        Content = content;
        TimeStamp = timeStamp;
    }

    public string Content { get; set; } = string.Empty;
    public DateTime TimeStamp { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? TimeOfDelete { get; set; }
}