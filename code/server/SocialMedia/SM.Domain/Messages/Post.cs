
namespace SM.Domain.Messages;

public class Post : Message
{
    protected Post() { }
    protected Post(Guid id, string content, DateTimeOffset timeStamp, Guid authorId)
        : base(id, content, timeStamp, authorId) { }

    public ICollection<Comment>? Replies { get; set; }

    public static Post Create(string content, Guid authorId)
    {
        var postId = Guid.CreateVersion7();
        var timeStamp = DateTimeOffset.Now;
        return new Post(postId, content, timeStamp, authorId);
    }
}
