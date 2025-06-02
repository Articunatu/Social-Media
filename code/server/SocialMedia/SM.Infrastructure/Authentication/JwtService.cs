using SM.Application.Authentication;
using System.Text;
using System.Security.Claims;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using SM.Domain.Authentication;

namespace SM.Infrastructure.Authentication;

internal class JwtService : IJwtService
{
    //void GeneratePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    //bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);

    public string CreateToken(string tag, string tokenValue)
    {
        List<Claim> claims =
        [
            new Claim(ClaimTypes.Name, tag),
            new Claim(ClaimTypes.Role, "Admin")
        ];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenValue));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    public Token GenerateRefreshToken()
    {
        var refreshToken = new Token(Guid.CreateVersion7())
        {
            Text = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.Now.AddDays(7),
            Created = DateTime.Now
        };
        return refreshToken;
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
