using SocialMedia.Domain.Abstractions;

namespace SocialMedia.Domain.Reactions
{
    public abstract class Reaction : Entity<Guid>
    {
        public Reaction() { }
        public Reaction(Guid id) : base(id) { }
        public  ReactionType Type { get; set; }
        public  Guid MessageId { get; set; }
    }

    public sealed class ReactionNoSQL : Reaction { }

    public sealed class ReactionRelational : Reaction
    {
        public Guid UserId { get; set; }
    }
}
