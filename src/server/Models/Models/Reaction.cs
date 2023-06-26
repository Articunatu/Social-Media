using Models.SubModels;

namespace Models.Models
{
    public class Reaction
    {
        public ReactionType Type { get; set; }
        public Image Icon { get; set; }
    }

    public enum ReactionType
    {
        Star,
        Fire,
        Rain,
        Flower,
        Tree,
        Lightning,
        Cloud,
        Toxic,
        Snow,
        Stone,
        Darkness
    }
}
