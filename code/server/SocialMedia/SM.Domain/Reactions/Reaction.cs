using SM.Domain.Abstractions;
using SM.Domain.Messages;
using SM.Domain.Users;

namespace SM.Domain.Reactions;

public class Reaction : Entity<Guid>
{
    public Reaction() { }
    public Reaction(Guid id) : base(id) { }
    public ReactionType Type { get; set; }
    public Guid MessageId { get; set; }
    public Guid UserId { get; set; }
    public virtual Message Message { get; set; }
    public virtual User User { get; set; }
}
