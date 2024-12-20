using SocialMedia.Domain.Shared;
using SocialMedia.Domain.Users;
using SocialMedia.Domain.Users.Authentication;

namespace SocialMedia.Application.Users.RefreshToken
{
    internal sealed class RefreshTokenCommandHandler
    {
        private readonly IUserRepository db;
        readonly IAuthenticationService auth;

        public RefreshTokenCommandHandler(IUserRepository readRepository, IAuthenticationService authenticationService)
        {
            db = readRepository;
            auth = authenticationService;
        }

        public async Task<Result> Handle(
            RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {
            var token = auth.RefreshToken;
            string query = $"SELECT c.id, c.tag FROM c WHERE c.token = @partitionKey";
            var user = await db.GetSingle<User>(token, query);
            if (user is null || user.Token.Expires < DateTime.Now)
                return Result.Failure(new Error("Token expired or invalid."));
            var newToken = auth.NewRefreshToken(user.Tag);
            return Result.Success(newToken);
        }
    }
}
