
namespace SM.Domain.Messages;

public class Comment(Guid id, string content, DateTime timeStamp) : Post(id, content, timeStamp)
{
    public Guid ParentPostId { get; set; }
    public virtual Post ParentPost { get; set; } = default!;
}
