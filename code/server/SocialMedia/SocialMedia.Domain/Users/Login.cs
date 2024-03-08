
namespace SocialMedia.Domain.Users
{
    internal class Login
    {
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
