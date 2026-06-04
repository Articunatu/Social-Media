using SM.Domain.Authentication;
using System.Security.Claims;

namespace SM.Application.Authentication;

public interface IJwtService
{
    public string CreateToken(string id, string tag);

    public ClaimsPrincipal? ValidateToken(string token);

    public Token GenerateRefreshToken();

    public void GeneratePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);

    public bool VerifyPasswordHash(string password, byte[]? passwordHash, byte[]? passwordSalt);
}
