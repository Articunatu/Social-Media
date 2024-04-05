//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using SocialMedia.Domain.Abstractions;
//using SocialMedia.Domain.Shared;
//using SocialMedia.Domain.Users;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Security.Cryptography;
//using System.Text;

//namespace SocialMedia.Application.Authentication
//{
//    internal class AuthenticationService
//    {
//        readonly IUserReadRepository _userReadRepository;
//        readonly IUserWriteRepository _userWriteRepository;
//        private static IConfiguration _configuration;

//        public AuthenticationService(IUserReadRepository userRead, IUserWriteRepository userWrite, IConfiguration configuration)
//        {
//            _userReadRepository = userRead;
//            _userWriteRepository = userWrite;
//            _configuration = configuration;
//        }

//        public async Task<Result> LoginAsync(LoginModel request)
//        {
//            var query = "SELECT Tag, Login FROM c WHERE c.tag = @partitionKey";
//            var account = await _userReadRepository.GetSingle<User>(request.Tag, KeyTypes.Tag, query);
//            if (account == null)
//            {
//                return Result.Failure(new Error($"Could not find an account with tag \"{request.Tag}\"."));
//            }
//            if (!VerifyPasswordHash(request.Password, account.Login.PasswordHash, account.Login.PasswordSalt))
//            {
//                return Result.Failure(new Error("Incorrect password."));
//            }
//            string token = CreateToken(account);
//            //API
//        }

//        private static void GeneratePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
//        {
//            using var secutiry = new HMACSHA512();
//            passwordSalt = secutiry.Key;
//            passwordHash = secutiry.ComputeHash(Encoding.UTF8.GetBytes(password));
//        }

//        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
//        {
//            using var hmac = new HMACSHA512(passwordSalt);
//            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
//            return computedHash.SequenceEqual(passwordHash);
//        }

//        private static string CreateToken(User user)
//        {
//            List<Claim> claims = new List<Claim>
//            {
//                new Claim(ClaimTypes.Name, user.Tag),
//                new Claim(ClaimTypes.Role, "Admin")
//            };
//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
//                _configuration.GetSection("AppSettings:Token").Value));
//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
//            var token = new JwtSecurityToken(
//                claims: claims,
//                expires: DateTime.Now.AddDays(1),
//                signingCredentials: creds);
//            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
//            return jwt;
//        }
//    }
//}
