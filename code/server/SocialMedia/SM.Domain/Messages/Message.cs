using SM.Domain.Abstractions;
using SM.Domain.Reactions;
using SM.Domain.Users;

namespace SM.Domain.Messages;

public abstract class Message : Entity<Guid>, ISoftDeletable
{
    protected Message() { }

    protected Message(Guid id, string content, DateTimeOffset timeStamp, Guid authorId)
    {
        Id = id;
        Content = content;
        TimeStamp = timeStamp;
        AuthorId = authorId;
    }

    public string Content { get; set; }
    public DateTimeOffset TimeStamp { get; set; }
    public Guid AuthorId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? TimeOfDelete { get; set; }
    public virtual User Author { get; set; } = default!;
    public virtual ICollection<Reaction>? Reactions { get; set; }
}