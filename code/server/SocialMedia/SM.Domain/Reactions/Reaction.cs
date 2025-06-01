using SM.Domain.Abstractions;
using SM.Domain.Messages;
using SM.Domain.Users;

namespace SM.Domain.Reactions;

public class Reaction(Guid id) : Entity<Guid>(id)
{
    public ReactionType Type { get; set; }
    public Guid MessageId { get; set; }
    public Guid UserId { get; set; }
    public virtual Message Message { get; set; } = default!;
    public virtual User User { get; set; } = default!;
}
