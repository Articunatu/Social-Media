using SM.Application.Abstractions;
using SM.Application.Authentication.Authorize.Models;
using SM.Application.Behaviors;
using SM.Domain.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace SM.Application.Authentication.Authorize;

internal class AuthorizeCommandHandler(ILoggingBehaviour logging) : ICommandHandler<AuthorizeCommand, Result<AuthorizeResponse>>
{
    public async Task<Result<Result<AuthorizeResponse>>> Handle(AuthorizeCommand request, CancellationToken cancellationToken)
    {
        var handler = new JwtSecurityTokenHandler();
        try
        {
            var jwtToken = handler.ReadJwtToken(request.AccessToken);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userName = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Result.Success(Result.Failure<AuthorizeResponse>(new Error("Unauthorized", "Invalid token or userId"), HttpStatusCode.Unauthorized));

            return Result.Success(Result.Success(new AuthorizeResponse(IsAuthorized: true, userId, userName)));
        }
        catch
        {
            logging.LogError($"Token validation failed for token: {request.AccessToken}");
            return Result.Success(Result.Failure<AuthorizeResponse>(new Error("Unauthorized", "Token validation failed"), HttpStatusCode.Unauthorized));
        }
    }
}
