namespace SM.Domain.Users.Authentication
{
    public sealed class LoginInformation
    {
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
