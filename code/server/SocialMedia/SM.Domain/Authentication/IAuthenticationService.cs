namespace SM.Domain.Authentication
{
    public interface IAuthenticationService
    {
        void GeneratePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        string CreateToken(string tag);
        Token GenerateRefreshToken();
        void SetRefreshToken(Token newRefreshToken);
        string? RefreshToken(string tag);
        string? NewRefreshToken(string tag);
        Guid GetLoggedInUserId();
        string GetLoginTag();
    }
}
