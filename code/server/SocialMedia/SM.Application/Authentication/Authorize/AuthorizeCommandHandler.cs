using SM.Application.Abstractions;
using SM.Application.Authentication;
using SM.Application.Authentication.Authorize.Models;
using SM.Application.Behaviors;
using SM.Domain.Shared;
using System.Net;
using System.Security.Claims;

namespace SM.Application.Authentication.Authorize;

internal class AuthorizeCommandHandler(IJwtService jwtService, ILoggingBehaviour logging) : ICommandHandler<AuthorizeCommand, Result<AuthorizeResponse>>
{
    public Task<Result<Result<AuthorizeResponse>>> Handle(AuthorizeCommand request, CancellationToken cancellationToken)
    {
        var principal = jwtService.ValidateToken(request.AccessToken);
        var userId = principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userName = principal?.FindFirst(ClaimTypes.Name)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            logging.LogWarning("Token validation failed during authorization");
            return Task.FromResult(Result.Success(Result.Failure<AuthorizeResponse>(new Error("Unauthorized", "Invalid token or userId"), HttpStatusCode.Unauthorized)));
        }

        return Task.FromResult(Result.Success(Result.Success(new AuthorizeResponse(IsAuthorized: true, userId, userName))));
    }
}
