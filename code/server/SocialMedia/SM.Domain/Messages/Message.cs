using SM.Domain.Abstractions;
using SM.Domain.Reactions;
using SM.Domain.Users;

namespace SM.Domain.Messages;

public abstract class Message(Guid id, string content, DateTimeOffset timeStamp, Guid authorId) : Entity<Guid>(id), ISoftDeletable
{
    protected Message() { }
    public string Content { get; set; } = content;
    public DateTimeOffset TimeStamp { get; set; } = timeStamp;
    public bool IsDeleted { get; set; }
    public DateTime? TimeOfDelete { get; set; }
    public Guid AuthorId { get; set; } = authorId;
    public virtual User Author { get; set; } = default!;
    public ICollection<Reaction>? Reactions { get; set; }
}