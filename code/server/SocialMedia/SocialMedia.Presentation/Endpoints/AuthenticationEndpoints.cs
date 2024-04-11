using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Application.Users.Commands.AddUserCommand;
using SocialMedia.Application.Users.Commands.LogInUser;
using SocialMedia.Domain.Abstractions;
using SocialMedia.Domain.Users;
using SocialMedia.Presentation.Endpoints.Profile;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using SocialMedia.Domain.Shared;

namespace SocialMedia.Presentation.Endpoints.Authentication
{
    public static class AuthenticationEndpoints
    {
        private static IConfiguration _configuration;
        private static IHttpContextAccessor _httpContextAccessor;

        public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/profiles");

            app.MapPost("{request}", SignUp);
            app.MapGet("{request}", LoginAsync);
            app.MapGet("", GetLoggedInUserId);
        }

        public static async Task<IResult> SignUp(
            AddUserRequest request,
            CancellationToken cancellationToken,
            ISender sender)
        {
            var command = new AddUserCommand(
                request.Tag,
                request.Email,
                request.FirstName,
                request.LastName,
                request.Password
            );

            var result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return TypedResults.BadRequest(result.Error);
            }

            return TypedResults.Ok(result.Value);
        }

        public static async Task<ActionResult<object>> LoginAsync(
            LogInUserRequest request,
            ISender sender)
        {
            var userResponse = await sender.Send(new LogInUserCommand(request.Email, request.Password, KeyTypes.Email));

            if (userResponse == null)
            {
                return TypedResults.BadRequest($"Could not find an account with tag \"{request.Tag}\".");
            }

            string token = CreateToken(request.Tag);

            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken);

            return new { accessToken = token };
        }


        private static string CreateToken(string tag)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, tag),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private static RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Text = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };
            return refreshToken;
        }

        private static void SetRefreshToken(RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", newRefreshToken.Text, cookieOptions);
        }

        public static async Task<Result<string>> RefreshToken(ISender sender)
        {
            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return Result.Failure<string>(new Error("Invalid Refresh Token."));
            string token = CreateToken(account);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);
            return Result.Success(token);
        }

        public static Result<string> GetLoginTag()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }
            return Result.Success(result);
        }

        public static async Task<Guid> GetLoggedInUserId()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (Guid.TryParse(userIdClaim, out Guid userId))
                {
                    return userId;
                }
            }
            return Guid.Empty;
        }
    }
}