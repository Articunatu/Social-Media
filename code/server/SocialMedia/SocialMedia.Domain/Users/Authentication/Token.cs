namespace SocialMedia.Domain.Users.Authentication
{
    public class Token
    {
        public User User { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Expires { get; set; }
        public Guid UserId { get; set; }
    }
}