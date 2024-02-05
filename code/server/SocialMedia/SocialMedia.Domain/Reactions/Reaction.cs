using SocialMedia.Domain.Abstractions;

namespace SocialMedia.Domain.Reactions
{
    public sealed class Reaction : Entity<Guid>
    {
        public Reaction() { }
        public Reaction(Guid id) : base(id) { }
        public  ReactionType Type { get; set; }
        public  Guid UserId { get; set; }
        public  Guid MessageId { get; set; }
    }
}
