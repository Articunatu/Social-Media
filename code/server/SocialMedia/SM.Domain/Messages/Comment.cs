
namespace SM.Domain.Messages;

public class Comment : Message
{
    protected Comment() { }

    private Comment(Guid id, string content, DateTimeOffset timestamp, Guid authorId, Guid parentPostId, Guid? parentCommentId)
        : base(id, content, timestamp, authorId)
    {
        ParentPostId = parentPostId;
        ParentCommentId = parentCommentId;
    }

    public Guid ParentPostId { get; private set; }
    public virtual Post ParentPost { get; private set; } = default!;

    public Guid? ParentCommentId { get; private set; }
    public virtual Comment? ParentComment { get; private set; }

    public virtual ICollection<Comment> Comments { get; private set; } = [];

    public static Comment Create(Guid postId, string content, Guid authorId, Guid? parentCommentId = null)
    {
        var commentId = Guid.CreateVersion7();
        var timestamp = DateTimeOffset.UtcNow;

        return new Comment(commentId, content, timestamp, authorId, postId, parentCommentId);
    }
}
