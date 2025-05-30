using SM.Domain.Abstractions;

namespace SM.Domain.Reactions;

public class Reaction : Entity<Guid>
{
    public Reaction() { }
    public Reaction(Guid id) : base(id) { }
    public ReactionType Type { get; set; }
    public Guid MessageId { get; set; }
    public Guid UserId { get; set; }
}
