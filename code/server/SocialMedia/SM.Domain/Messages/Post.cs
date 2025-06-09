
namespace SM.Domain.Messages;

public class Post(Guid id, string content, DateTimeOffset timeStamp, Guid authorId) : Message(id, content, timeStamp, authorId)
{
    protected Post() { }
    public ICollection<Comment>? Replies { get; set; }

    public static Post Create(string content, Guid authorId)
    {
        var postId = Guid.CreateVersion7();
        var timeStamp = DateTimeOffset.Now;
        return new Post(postId, content, timeStamp, authorId);
    }
}
