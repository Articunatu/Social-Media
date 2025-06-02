using MediatR;
using Microsoft.EntityFrameworkCore;
using SM.Application.Database;

namespace SM.Application.Authentication.Logout;

internal class LogoutCommandHandler(ApplicationDbContext context) : IRequestHandler<LogoutCommand, Unit>
{

    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var token = await context.Tokens.FirstOrDefaultAsync(t => t.Text == request.RefreshToken, cancellationToken);
        if (token is not null)
        {
            context.Tokens.Remove(token);
            await context.SaveChangesAsync(cancellationToken);
        }

        return Unit.Value;
    }
}
