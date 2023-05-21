using Models.SubModels;

namespace Models.Models
{
    public class Reaction
    {
        public ReactionType? Type { get; set; }
        public Image? Icon { get; set; }
    }

    public enum ReactionType
    {
        Star,
        Heart,
        Fire,
        Rain,
        Flower,
        Tree,
        Lightning,
        Toxic,
        Snow,
        Stone,
        Darkness
    }
}
