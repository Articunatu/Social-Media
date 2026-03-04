using SM.Domain.Authentication;

namespace SM.Application.Authentication;

public interface IJwtService
{
    public string CreateToken(string id, string tag, string tokenValue);

    public Token GenerateRefreshToken();

    public void GeneratePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);

    public bool VerifyPasswordHash(string password, byte[]? passwordHash, byte[]? passwordSalt);
}
