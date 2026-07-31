using SM.Domain.Messages;
using SM.Domain.Reactions;
using SM.Domain.Users;

namespace SM.Domain.Content;

public class Post(Guid id) : Message(id)
{
    public virtual User Author { get; set; } = default!;
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
