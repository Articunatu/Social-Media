using SocialMedia.Application.Abstractions;
using SocialMedia.Domain.Shared;
using SocialMedia.Domain.Users;
using SocialMedia.Domain.Users.Authentication;

namespace SocialMedia.Application.Users.LogInUser
{
    internal sealed class LogInUserCommandHandler : ICommandHandler<LogInUserCommand, object>
    {
        private readonly IUserRepository db;
        readonly IAuthenticationService auth;

        public LogInUserCommandHandler(IUserRepository readRepository, IAuthenticationService authenticationService)
        {
            db = readRepository;
            auth = authenticationService;
        }

        //Result<object>
        public async Task<Result<object>> Handle(
            LogInUserCommand request,
            CancellationToken cancellationToken)
        {
            string query = $"SELECT c.id, c.tag, c.email, c.passwordHash, c.passwordSalt FROM c WHERE c.email = @partitionKey";
            var user = await db.GetSingle<User>(request.Email, query);

            if (user is null)
                return Result.Failure<object>(new Error($"Could not find an account with email {request.Email}"));

            if (user.PasswordHash is null)
                return Result.Failure<object>(new Error($"No password hash"));

            if (request.Password is null)
                return Result.Failure<object>(new Error($"Password empty"));

            if (!auth.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                return Result.Failure<object>(new Error("Incorrect password."));

            string token = auth.CreateToken(user.Tag);

            var newRefreshToken = auth.GenerateRefreshToken();
            auth.SetRefreshToken(newRefreshToken);

            return Result.Success(new { accessToken = token });

            //if (string.IsNullOrEmpty(refreshToken))
            //    return new Error("Invalid Refresh Token.");
        }
    }
}