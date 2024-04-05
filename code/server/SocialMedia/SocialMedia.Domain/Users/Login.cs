
namespace SocialMedia.Domain.Users
{
    public sealed class Login
    {
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
