using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Domain.Shared;

namespace SM.Application.Reactions.RemoveReaction;

internal class RemoveReactionCommandHandler(ApplicationDbContext context) 
    : ICommandHandler<RemoveReactionCommand, ReactionResponse>
{
    public async Task<Result<ReactionResponse>> Handle(RemoveReactionCommand request, CancellationToken cancellationToken)
    {
        var reactionToRemove = await context.Reactions
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken: cancellationToken);

        if (reactionToRemove is null)
            return Result.Failure<ReactionResponse>(new Error("NotFound"));

        context.Reactions.Remove(reactionToRemove);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(reactionToRemove.MapToResponse());
    }
}
