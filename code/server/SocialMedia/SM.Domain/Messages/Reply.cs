
namespace SM.Domain.Messages;

public sealed class Reply : Post
{
    public Guid ParentPostId { get; set; }
}
