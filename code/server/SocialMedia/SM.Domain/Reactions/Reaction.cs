using SM.Domain.Abstractions;
using SM.Domain.Content;
using SM.Domain.Users;

namespace SM.Domain.Reactions;

public class Reaction(Guid id) : Entity<Guid>(id)
{
    public ReactionType Type { get; set; }
    public Guid UserId { get; set; }
    public Guid? PostId { get; set; }
    public Guid? CommentId { get; set; }

    public virtual User User { get; set; } = default!;
    public virtual Post? Post { get; set; }
    public virtual Comment? Comment { get; set; }
}
