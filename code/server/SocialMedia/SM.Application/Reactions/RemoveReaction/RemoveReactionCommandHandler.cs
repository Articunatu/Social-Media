using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Domain.Shared;
using System.Net;

namespace SM.Application.Reactions.RemoveReaction;

internal class RemoveReactionCommandHandler(IDbContextFactory<ApplicationDbContext> contextFactory) 
    : ICommandHandler<RemoveReactionCommand, ReactionResponse>
{
    public async Task<Result<ReactionResponse>> Handle(RemoveReactionCommand request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var reactionToRemove = await context.Reactions
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (reactionToRemove is null)
            return Result.Failure<ReactionResponse>(new Error("NotFound"), HttpStatusCode.NotFound);

        context.Reactions.Remove(reactionToRemove);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(reactionToRemove.MapToResponse());
    }
}
