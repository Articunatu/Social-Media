using SocialMedia.Application.Abstractions.Authentication;
using SocialMedia.Application.Abstractions;
using SocialMedia.Domain.Shared;
using SocialMedia.Domain.Users;
using System.Security.Principal;
using System.Security.Cryptography;
using System.Text;

namespace SocialMedia.Application.Users.Commands.LogInUser
{
    internal sealed class LogInUserCommandHandler : ICommandHandler<LogInUserCommand, AccessTokenResponse>
    {
        private readonly IUserReadRepository _readRepository;

        public LogInUserCommandHandler(IUserReadRepository readRepository)
        {
            _readRepository = readRepository;
        }

        public async Task<Result<AccessTokenResponse>> Handle(
            LogInUserCommand request,
            CancellationToken cancellationToken)
        {
            string query = $"SELECT c.id, c.tag c.email FROM c WHERE {request.Email} = @partitionKey";
            var user = await _readRepository.GetSingle<User>(request.Email, query);
            if (!VerifyPasswordHash(request.Password, user.LoginInformation.PasswordHash, user.LoginInformation.PasswordSalt))
            {
                return Result.Failure<AccessTokenResponse>(new Error("Incorrect password."));
            }
            return Result.Success(user);
        }
        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}