using SocialMedia.Application.Abstractions;
using SocialMedia.Domain.Shared;
using SocialMedia.Domain.Users;

namespace SocialMedia.Application.Users.Commands.LogInUser
{
    internal sealed class LogInUserCommandHandler : ICommandHandler<LogInUserCommand, object>
    {
        private readonly IUserReadRepository db;
        readonly IAuthenticationService auth;

        public LogInUserCommandHandler(IUserReadRepository readRepository, IAuthenticationService authenticationService)
        {
            db = readRepository;
            auth = authenticationService;
        }

        //Result<object>
        public async Task<Result<object>> Handle(
            LogInUserCommand request,
            CancellationToken cancellationToken)
        {
            string query = $"SELECT c.id, c.tag c.email FROM c WHERE c.email = @partitionKey";
            var user = await db.GetSingle<User>(request.Email, query);

            if (user is null)
                return Result.Failure<object>(new Error("Could not find an account with tag"));

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