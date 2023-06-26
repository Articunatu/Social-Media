using API.ViewModels;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models.Models;
using Models.SubModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        readonly IConfiguration _configuration;
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly IAccountRepository _accountRepository;

        public AuthenticationController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IAccountRepository accountRepository)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _accountRepository = accountRepository;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(LoginModel request)
        {
            Account account = new();
            GeneratePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            account.Login = new Login
            {
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _accountRepository.AddNewAccount(account);

            return Ok(account);
        }

        private static void GeneratePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var secutiry = new HMACSHA512();
            passwordSalt = secutiry.Key;
            passwordHash = secutiry.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginAsync(LoginModel request)
        {
            var account = await _accountRepository.GetAccountByTag(request.Tag);

            if (account == null)
            {
                return BadRequest($"Could not find an account with tag \"{request.Tag}\".");
            }

            if (!VerifyPasswordHash(request.Password, account.Login.PasswordHash, account.Login.PasswordSalt))
            {
                return BadRequest("Incorrect password.");
            }

            string token = CreateToken(account);

            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken);

            return Ok(token);
        }
        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }

        private string CreateToken(Account account)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.Tag),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
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

        private void SetRefreshToken(RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Text, cookieOptions);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token.");
            }

            var account = await _accountRepository.GetAccountByToken(refreshToken);

            if (account == null || account.Token.Expires < DateTime.Now)
            {
                return Unauthorized("Token expired or invalid.");
            }

            string token = CreateToken(account);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);

            return Ok(token);
        }

        [HttpGet, Authorize]
        public ActionResult<string> GetLoginTag()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }
            return Ok(result);
        }

        [HttpGet("loggedin-id")]
        public async Task<Guid> GetLoggedInAccountId()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                var accountIdClaim = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (Guid.TryParse(accountIdClaim, out Guid accountId))
                {
                    return accountId;
                }
            }
            return Guid.Empty;
        }
    }
}