using SM.Domain.Abstractions;

namespace SM.Domain.Content;

public class Reaction(Guid id) : Entity<Guid>(id)
{
    public ReactionType Type { get; set; }
    public Guid UserId { get; set; }
    public Guid? PostId { get; set; }
    public Guid? CommentId { get; set; }

    public virtual Post? Post { get; set; }
    public virtual Comment? Comment { get; set; }
}
