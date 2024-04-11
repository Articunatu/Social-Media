using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Domain.Abstractions;
using SocialMedia.Domain.Shared;
using SocialMedia.Domain.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SocialMedia.Application.Authentication
{
    internal class AuthenticationService
    {
        readonly IUserReadRepository _userReadRepository;
        readonly IUserWriteRepository _userWriteRepository;
        private static IConfiguration _configuration;

        public AuthenticationService(IUserReadRepository userRead, IUserWriteRepository userWrite, IConfiguration configuration)
        {
            _userReadRepository = userRead;
            _userWriteRepository = userWrite;
            _configuration = configuration;
        }

        
    }
}
