using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SM.Application.Authentication;
using SM.Domain.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SM.Infrastructure.Authentication;

public class JwtService(IConfiguration configuration) : IJwtService
{
    private readonly string _tokenKey = configuration["JwtSettings:TokenKey"] 
        ?? throw new NotImplementedException("Token not registered!");

    public string CreateToken(string id, string tag)
    {
        List<Claim> claims =
        [
            new Claim(ClaimTypes.NameIdentifier, id),
            new Claim(ClaimTypes.Name, tag),
            new Claim(ClaimTypes.Role, "Creator")
        ];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenKey));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_tokenKey);

        try
        {
            return tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out _);
        }
        catch
        {
            return null;
        }
    }

    public Token GenerateRefreshToken(Guid userId)
    {
        var now = DateTimeOffset.UtcNow;
        return Token.Create(
            userId,
            Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            now,
            now.AddDays(7));
    }

    public void GeneratePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var secutiry = new HMACSHA512();
        passwordSalt = secutiry.Key;
        passwordHash = secutiry.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    public bool VerifyPasswordHash(string password, byte[]? passwordHash, byte[]? passwordSalt)
    {
        if (passwordHash is null || passwordSalt is null)
            return false;

        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHash);
    }
}
