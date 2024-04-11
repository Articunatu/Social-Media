namespace SocialMedia.Domain.Users
{
    public class RefreshToken
    {
        public string Text { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Expires { get; set; }
    }
}