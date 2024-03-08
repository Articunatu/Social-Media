//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Security.Cryptography;

//namespace SocialMedia.Presentation.Endpoints.Authentication
//{
//    public static class AuthenticationEndpoints
//    {
//        private static IConfiguration _configuration;
//        private static IHttpContextAccessor _httpContextAccessor;
//        private static IAccountRepository _accountRepository;

//        public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder app, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IAccountRepository accountRepository)
//        {
//            _configuration = configuration;
//            _httpContextAccessor = httpContextAccessor;
//            _accountRepository = accountRepository;

//            var group = app.MapGroup("api/authentication");

//            group.MapPost("signup", SignUp);
//            group.MapPost("login", LoginAsync);
//            group.MapPost("refresh-token", RefreshToken);
//            group.MapGet("", GetLoginTag);
//            group.MapGet("logged-in-id", GetLoggedInAccountId);
//        }

//        private static async Task<IResult> SignUp(LoginModel request)
//        {
//            Account account = new()
//            {
//                Tag = request.Tag
//            };

//            GeneratePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

//            account.Login = new Login
//            {
//                PasswordHash = passwordHash,
//                PasswordSalt = passwordSalt
//            };

//            await _accountRepository.AddNewAccount(account);

//            return TypedResults.Ok(account);
//        }

//        private static async Task<IResult> LoginAsync(LoginModel request)
//        {
//            var account = await _accountRepository.GetAccountByTag(request.Tag);

//            if (account == null)
//            {
//                return TypedResults.BadRequest($"Could not find an account with tag \"{request.Tag}\".");
//            }

//            if (!VerifyPasswordHash(request.Password, account.Login.PasswordHash, account.Login.PasswordSalt))
//            {
//                return TypedResults.BadRequest("Incorrect password.");
//            }

//            string token = CreateToken(account);

//            var refreshToken = GenerateRefreshToken();
//            SetRefreshToken(refreshToken);

//            return TypedResults.Ok(new { accessToken = token });
//        }

//        private static async Task<IResult> RefreshToken()
//        {
//            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];

//            if (string.IsNullOrEmpty(refreshToken))
//            {
//                return TypedResults.Unauthorized("Invalid Refresh Token.");
//            }

//            var account = await _accountRepository.GetAccountByToken(refreshToken);

//            if (account == null || account.Token.Expires < DateTime.Now)
//            {
//                return TypedResults.Unauthorized("Token expired or invalid.");
//            }

//            string token = CreateToken(account);
//            var newRefreshToken = GenerateRefreshToken();
//            SetRefreshToken(newRefreshToken);

//            return TypedResults.Ok(token);
//        }

//        private static IResult GetLoginTag()
//        {
//            var result = string.Empty;
//            if (_httpContextAccessor.HttpContext != null)
//            {
//                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
//            }
//            return TypedResults.Ok(result);
//        }

//        private static async Task<IResult> GetLoggedInAccountId()
//        {
//            if (_httpContextAccessor.HttpContext != null)
//            {
//                var accountIdClaim = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
//                if (Guid.TryParse(accountIdClaim, out Guid accountId))
//                {
//                    return TypedResults.Ok(accountId);
//                }
//            }
//            return TypedResults.Ok(Guid.Empty);
//        }

//        private static void GeneratePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
//        {
//            using var secutiry = new HMACSHA512();
//            passwordSalt = secutiry.Key;
//            passwordHash = secutiry.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
//        }

//        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
//        {
//            using var hmac = new HMACSHA512(passwordSalt);
//            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
//            return computedHash.SequenceEqual(passwordHash);
//        }

//        private static string CreateToken(Account account)
//        {
//            List<Claim> claims = new List<Claim>
//        {
//            new Claim(ClaimTypes.Name, account.Tag),
//            new Claim(ClaimTypes.Role, "Admin")
//        };

//            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
//                _configuration.GetSection("AppSettings:Token").Value));

//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

//            var token = new JwtSecurityToken(
//                claims: claims,
//                expires: DateTime.Now.AddDays(1),
//                signingCredentials: creds);

//            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

//            return jwt;
//        }

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

//        private static void SetRefreshToken(RefreshToken newRefreshToken)
//        {
//            var cookieOptions = new CookieOptions
//            {
//                HttpOnly = true,
//                Expires = newRefreshToken.Expires
//            };
//            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", newRefreshToken.Text, cookieOptions);
//        }
//    }
//}
