using SM.Domain.Messages;
using SM.Domain.Users;

namespace SM.Domain.Content;

public class Comment(Guid id) : Message(id)
{
    public Guid ParentPostId { get; private set; }
    public virtual Post ParentPost { get; private set; } = default!;
    public Guid? ParentCommentId { get; private set; }
    public virtual Comment? ParentComment { get; private set; }
    public virtual ICollection<Comment> Replies { get; private set; } = [];
    public virtual ICollection<Reaction> Reactions { get; private set; } = [];
    public virtual User Author { get; set; } = default!;

    public static Comment Create(Guid postId, string content, Guid authorId, Guid? parentCommentId = null)
    {
        return new Comment(Guid.CreateVersion7())
        {
            Content = content,
            TimeStamp = DateTimeOffset.UtcNow,
            AuthorId = authorId,
            ParentPostId = postId,
            ParentCommentId = parentCommentId
        };
    }
}
