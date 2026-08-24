using SM.Domain.Content.Events;

namespace SM.Domain.Content;

public class Post(Guid id) : AuthoredContent(id)
{
    public virtual ICollection<Comment> Comments { get; private set; } = [];
    public virtual ICollection<Reaction> Reactions { get; private set; } = [];

    public static Post Create(string content, Guid authorId)
    {
        var post = new Post(Guid.CreateVersion7())
        {
            Content = content,
            TimeStamp = DateTimeOffset.UtcNow,
            AuthorId = authorId,
        };

        post.RaiseDomainEvent(new PostCreatedDomainEvent(post.Id, authorId, post.TimeStamp));

        return post;
    }
}
