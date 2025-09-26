using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Domain.Shared;
using System.Net;

namespace SM.Application.Authentication.RefreshToken;

internal class RefreshTokenCommandHandler(IJwtService jwtService, IConfiguration config, IDbContextFactory<ApplicationDbContext> contextFactory) 
    : ICommandHandler<RefreshTokenCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var existing = await context.Tokens
            .FirstOrDefaultAsync(t => t.Text == request.RefreshToken, cancellationToken);

        if (existing == null || existing.Expires < DateTime.UtcNow)
            return Result.Failure<LoginResponse>(new Error("Invalid or expired refresh token"), HttpStatusCode.BadRequest);

        string accessToken = jwtService.CreateToken(existing.UserId.ToString(), config["AppSettings:Token"]!);
        var refreshedToken = jwtService.GenerateRefreshToken();
        refreshedToken.UserId = existing.UserId;

        context.Tokens.Remove(existing);
        context.Tokens.Add(refreshedToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(new LoginResponse(accessToken, refreshedToken));
    }
}
