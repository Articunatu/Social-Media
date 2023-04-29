using Models.Models;

namespace Models.SubModels.Account
{
    public class ReactedPost
    {
        public Post Post { get; set; }
        public ReactionType Reaction { get; set; }
    }
}
