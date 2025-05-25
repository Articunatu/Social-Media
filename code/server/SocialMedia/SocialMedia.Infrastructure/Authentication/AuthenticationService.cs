//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using SocialMedia.Domain.Users.Authentication;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Security.Cryptography;
//using System.Text;

//namespace SocialMedia.Infrastructure.Authentication;

//internal class AuthenticationService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : IAuthenticationService
//{
//    public string GetLoginTag()
//    {
//        var result = string.Empty;
//        if (httpContextAccessor.HttpContext != null)
//            result = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
//        return result;
//    }

//    public Guid GetLoggedInUserId()
//    {
//        if (httpContextAccessor.HttpContext != null)
//        {
//            var userIdClaim = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
//            if (Guid.TryParse(userIdClaim, out Guid userId))
//                return userId;
//        }
//        return Guid.Empty;
//    }

//    public string CreateToken(string tag)
//    {
//        List<Claim> claims =
//        [
//            new Claim(ClaimTypes.Name, tag),
//            new Claim(ClaimTypes.Role, "Admin")
//        ];

//        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
//            configuration.GetSection("AppSettings:Token").Value));

//        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

//        var token = new JwtSecurityToken(
//            claims: claims,
//            expires: DateTime.Now.AddDays(1),
//            signingCredentials: creds);

//        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

//        return jwt;
//    }

//    public Token GenerateRefreshToken()
//    {
//        var refreshToken = new Token
//        {
//            Text = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
//            Expires = DateTime.Now.AddDays(7),
//            Created = DateTime.Now
//        };
//        return refreshToken;
//    }

//    public void SetRefreshToken(Token newRefreshToken)
//    {
//        var cookieOptions = new CookieOptions
//        {
//            HttpOnly = true,
//            Expires = newRefreshToken.Expires
//        };
//        httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", newRefreshToken.Text, cookieOptions);
//    }

//    public string? RefreshToken(string tag)
//    {
//        return httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
//    }

//    public string? NewRefreshToken(string tag)
//    {
//        string token = CreateToken(tag);
//        var newRefreshToken = GenerateRefreshToken();
//        SetRefreshToken(newRefreshToken);
//        return token;
//    }

//    public void GeneratePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
//    {
//        using var secutiry = new HMACSHA512();
//        passwordSalt = secutiry.Key;
//        passwordHash = secutiry.ComputeHash(Encoding.UTF8.GetBytes(password));
//    }

//    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
//    {
//        using var hmac = new HMACSHA512(passwordSalt);
//        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
//        return computedHash.SequenceEqual(passwordHash);
//    }
//}