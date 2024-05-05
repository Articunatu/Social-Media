namespace SocialMedia.Domain.Users
{
    public interface IAuthenticationService
    {
        void GeneratePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        string CreateToken(string tag);
        RefreshToken GenerateRefreshToken();
        void SetRefreshToken(RefreshToken newRefreshToken);
        string? RefreshToken(string tag);
        Guid GetLoggedInUserId();
        string GetLoginTag();
    }
}
