
namespace SM.Domain.Messages;

public class Post(Guid id, string content, DateTime timeStamp) : Message(id, content, timeStamp)
{
    public ICollection<Comment>? Replies { get; set; }

    public static Post Create(string content, DateTime timeStamp)
    {
        var postId = Guid.CreateVersion7();
        return new Post(postId, content, timeStamp);
    }
}
