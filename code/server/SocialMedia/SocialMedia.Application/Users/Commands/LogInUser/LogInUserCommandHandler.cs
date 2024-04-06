using SocialMedia.Application.Abstractions.Authentication;
using SocialMedia.Application.Abstractions;
using SocialMedia.Domain.Shared;
using SocialMedia.Domain.Users;

namespace SocialMedia.Application.Users.Commands.LogInUser
{
    internal sealed class LogInUserCommandHandler : ICommandHandler<LogInUserCommand, AccessTokenResponse>
    {
        private readonly IJwtService _jwtService;

        public LogInUserCommandHandler(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public async Task<Result<AccessTokenResponse>> Handle(
            LogInUserCommand request,
            CancellationToken cancellationToken)
        {
            var result = await _jwtService.GetAccessTokenAsync(
                request.Email,
                request.Password,
                cancellationToken);

            if (result.IsFailure)
                return Result.Failure<AccessTokenResponse>(UserErrors.InvalidCredentials);

            return new AccessTokenResponse(result.Value);
        }
    }
}