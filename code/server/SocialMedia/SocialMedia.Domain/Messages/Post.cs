using SocialMedia.Domain.Reactions;


namespace SocialMedia.Domain.Messages
{
    public class Post : Message
    {
        public ICollection<Reaction>? Reactions { get; set; }
        public ICollection<Reply>? Replys { get; set; }
    }
}
