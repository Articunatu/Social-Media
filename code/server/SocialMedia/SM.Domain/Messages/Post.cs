using SM.Domain.Abstractions;
using SM.Domain.Users;
using SM.Domain.Reactions;

namespace SM.Domain.Messages;

public class Post : Message
{
    public virtual User Author { get; set; } = default!;
    public virtual ICollection<Comment> Comments { get; set; } = [];
    public virtual ICollection<Reaction> Reactions { get; set; } = [];

    public static Post Create(string content, Guid authorId)
    {
        return new Post()
        {
            Id = Guid.CreateVersion7(),
            Content = content,
            TimeStamp = DateTimeOffset.UtcNow,
            AuthorId = authorId,
        };
    }
}
