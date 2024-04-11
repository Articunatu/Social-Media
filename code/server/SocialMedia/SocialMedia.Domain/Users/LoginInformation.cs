
namespace SocialMedia.Domain.Users
{
    public sealed class LoginInformation
    {
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
