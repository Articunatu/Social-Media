using MediatR;
using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Domain.Shared;

namespace SM.Application.Authentication.Logout;

internal class LogoutCommandHandler(IDbContextFactory<ApplicationDbContext> contextFactory) : ICommandHandler<LogoutCommand, Unit>
{
    public async Task<Result<Unit>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var token = await context.Tokens.FirstOrDefaultAsync(t => t.Text == request.RefreshToken, cancellationToken);
        if (token != null)
        {
            context.Tokens.Remove(token);
            await context.SaveChangesAsync(cancellationToken);
        }

        return Result.Success(Unit.Value);
    }
}
