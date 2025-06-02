using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SM.Application.Database;

namespace SM.Application.Authentication.RefreshToken;

internal class RefreshTokenCommandHandler(IJwtService jwtService, IConfiguration config, ApplicationDbContext context) 
    : IRequestHandler<RefreshTokenCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var existing = await context.Tokens
            .FirstOrDefaultAsync(t => t.Text == request.RefreshToken, cancellationToken);

        if (existing == null || existing.Expires < DateTime.UtcNow)
            throw new UnauthorizedAccessException("Invalid or expired refresh token");

        string accessToken = jwtService.CreateToken(existing.UserId.ToString(), config["AppSettings:Token"]!);
        var refreshedToken = jwtService.GenerateRefreshToken();
        refreshedToken.UserId = existing.UserId;

        context.Tokens.Remove(existing);
        await context.Tokens.AddAsync(refreshedToken, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new LoginResponse(accessToken, refreshedToken);
    }
}
