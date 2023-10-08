using Domain.Enums;

namespace Domain.Entities
{
    public class Reaction : Entity
    {
        public Reaction(Guid id) : base(id) { }
        public ReactionType Type { get; set; }
    }
}
