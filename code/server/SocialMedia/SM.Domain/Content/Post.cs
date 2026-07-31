namespace SM.Domain.Content;

public class Post(Guid id) : AuthoredContent(id)
{
    public virtual ICollection<Comment> Comments { get; set; } = [];
    public virtual ICollection<Reaction> Reactions { get; set; } = [];

    public static Post Create(string content, Guid authorId)
    {
        return new Post(Guid.CreateVersion7())
        {
            Content = content,
            TimeStamp = DateTimeOffset.UtcNow,
            AuthorId = authorId,
        };
    }
}
