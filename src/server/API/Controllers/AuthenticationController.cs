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
        public static Account account = new();
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
            GeneratePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            account.Tag = request.Tag;
            account.Login.PasswordHash = passwordHash;
            account.Login.PasswordSalt = passwordSalt;

            await _accountRepository.Create(account);

            return Ok(account);
        }

        private void GeneratePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var secutiry = new HMACSHA512())
            {
                passwordSalt = secutiry.Key;
                passwordHash = secutiry.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginModel request)
        {
            if (account.Tag != request.Tag)
            {
                return BadRequest("Could not find the account.");
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


        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (!account.Token.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token.");
            }
            else if (account.Token.Expires < DateTime.Now)
            {
                return Unauthorized("Token expired.");
            }

            string token = CreateToken(account);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);

            return Ok(token);
        }

        private RefreshToken GenerateRefreshToken()
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

            account.Token.Text = newRefreshToken.Text;
            account.Token.Created = newRefreshToken.Created;
            account.Token.Expires = newRefreshToken.Expires;
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

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
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
    }
}