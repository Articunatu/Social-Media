
namespace SM.Domain.Messages;

public class Comment : Post
{
    protected Comment() { }

    public Comment(Guid id, string content, DateTime timestamp, Guid parentPostId, Guid authorId)
        : base(id, content, timestamp, authorId)
    {
        ParentPostId = parentPostId;
    }

    public Guid ParentPostId { get; private set; }
    public virtual Post ParentPost { get; private set; } = default!;

    public static Comment Create(Guid parentPostId, string content, DateTime timestamp, Guid authorId)
    {
        return new Comment(Guid.NewGuid(), content, timestamp, parentPostId, authorId);
    }
}
