
namespace SM.Domain.Messages;

public class Comment(Guid id, string content, DateTime timeStamp, Guid parentPostId, Guid authorId) : Post(id, content, timeStamp, authorId)
{
    public Guid ParentPostId { get; set; } = parentPostId;
    public virtual Post ParentPost { get; set; } = default!;

    public static Comment Create(Guid parentPostId, string content, DateTime timeStamp, Guid authorId)
    {
        return new Comment(Guid.CreateVersion7(), content, timeStamp, parentPostId, authorId);
    }
}