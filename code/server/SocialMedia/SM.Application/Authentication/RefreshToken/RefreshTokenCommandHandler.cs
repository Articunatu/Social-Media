using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SM.Application.Abstractions;
using SM.Application.Behaviors;
using SM.Application.Database;
using SM.Domain.Shared;
using System.Net;

namespace SM.Application.Authentication.RefreshToken;

internal class RefreshTokenCommandHandler(IJwtService jwtService, IConfiguration config, IDbContextFactory<ApplicationDbContext> contextFactory, ILoggingBehaviour logging) 
    : ICommandHandler<RefreshTokenCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        await using var context = await contextFactory.CreateDbContextAsync(ct);

        var existing = await context.Tokens.FirstOrDefaultAsync(t => t.Text == request.RefreshToken, ct);

        if (existing == null || existing.Expires < DateTime.UtcNow)
            return Result.Failure<LoginResponse>(new Error("Invalid or expired refresh token"), HttpStatusCode.BadRequest);

        string accessToken = jwtService.CreateToken(existing.UserId.ToString(), "", config["AppSettings:Token"]!);
        var refreshedToken = jwtService.GenerateRefreshToken();
        refreshedToken.UserId = existing.UserId;

        existing.Text = refreshedToken.Text;
        existing.Expires = refreshedToken.Expires;
        await context.SaveChangesAsync(ct);

        logging.LogInformation($"Refreshed token for user {existing.UserId}");
        return Result.Success(new LoginResponse(accessToken, refreshedToken));
    }
}
