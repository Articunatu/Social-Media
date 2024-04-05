//using MediatR;
//using Microsoft.IdentityModel.Tokens;
//using SocialMedia.Application.Authentication;
//using SocialMedia.Application.Users.Commands.AddUser;
//using SocialMedia.Application.Users.Queries.GetUserById;
//using SocialMedia.Domain.Users.ValueObjects;
//using System.IdentityModel.Tokens.Jwt;
//using System.Reflection;
//using System.Security.Claims;
//using System.Security.Cryptography;

//namespace SocialMedia.Presentation.Endpoints.Authentication
//{
//    public class RefreshToken
//    {
//        public string Text { get; set; } = string.Empty;
//        public DateTime Created { get; set; } = DateTime.Now;
//        public DateTime Expires { get; set; }
//    }

//    public static class AuthenticationEndpoints
//    {
//        private static IConfiguration configuration;
//        private static IHttpContextAccessor httpContextAccessor;

//        public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder app)
//        {
//            var group = app.MapGroup("api/profiles");

//            app.MapPost("{request}", SignUp);
//            app.MapGet("{request}", LoginAsync);
//        }

//        public static async Task<IResult> SignUp(LoginModel request, ISender sender)
//        {
//            var command = new AddUserCommand(
//                request.Tag,
//                request.Email,
//                request.FirstName,
//                request.LastName
//                );
//            try
//            {
//                await sender.Send(command);
//                GeneratePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
//                account.Login = new Login
//                {
//                    PasswordHash = passwordHash,
//                    PasswordSalt = passwordSalt
//                };
//                return TypedResults.Ok();
//            }
//            catch (Exception e)
//            {
//                return TypedResults.BadRequest(e.Message);
//            }

//            //await _accountRepository.AddNewAccount(account);
//        }

//        public static async Task<IResult> LoginAsync(ISender sender)
//        {
//            //var user = await sender.Send(new GetUserByIdQuery(tag));
//            var refreshToken = GenerateRefreshToken();
//            SetRefreshToken(refreshToken, IHttpContextAccessor httpContextAccessor);
//            return TypedResults.Ok(new { accessToken = token });
//        }
//        //API
//        private static async Task<IResult> RefreshToken(IHttpContextAccessor httpContextAccessor, ISender sender)
//        {
//            var refreshToken = httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];

//            if (string.IsNullOrEmpty(refreshToken))
//            {
//                return TypedResults.Unauthorized("Invalid Refresh Token.");
//            }

//            var account = await sender.Send(new GetUserByIdQuery(refreshToken, Domain.Abstractions.KeyTypes.Token));

//            if (account == null || account.Expires < DateTime.Now)
//            {
//                return TypedResults.Unauthorized("Token expired or invalid.");
//            }

//            string token = CreateToken(account);
//            var newRefreshToken = GenerateRefreshToken();
//            SetRefreshToken(newRefreshToken);

//            return TypedResults.Ok(token);
//        }

//        private static IResult GetLoginTag(IHttpContextAccessor httpContextAccessor)
//        {
//            var result = string.Empty;
//            if (httpContextAccessor.HttpContext != null)
//            {
//                result = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
//            }
//            return TypedResults.Ok(result);
//        }

//        private static async Task<IResult> GetLoggedInAccountId(IHttpContextAccessor httpContextAccessor)
//        {
//            if (httpContextAccessor.HttpContext != null)
//            {
//                var accountIdClaim = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
//                if (Guid.TryParse(accountIdClaim, out Guid accountId))
//                {
//                    return TypedResults.Ok(accountId);
//                }
//            }
//            return TypedResults.Ok(Guid.Empty);
//        }

//        private static void SetRefreshToken(RefreshToken newRefreshToken, IHttpContextAccessor httpContextAccessor)
//        {
//            var cookieOptions = new CookieOptions
//            {
//                HttpOnly = true,
//                Expires = newRefreshToken.Expires
//            };
//            httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", newRefreshToken.Text, cookieOptions);
//        }

//        //Infra
//        private static RefreshToken GenerateRefreshToken()
//        {
//            var refreshToken = new RefreshToken
//            {
//                Text = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
//                Expires = DateTime.Now.AddDays(7),
//                Created = DateTime.Now
//            };

//            return refreshToken;
//        }
//    }
//}
