using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Domain.Shared;
using System.Net;

namespace SM.Application.Reactions.UpdateReaction;

internal class UpdateReactionCommandHandler(IDbContextFactory<ApplicationDbContext> contextFactory) : ICommandHandler<UpdateReactionCommand, ReactionResponse>
{
    public async Task<Result<ReactionResponse>> Handle(UpdateReactionCommand request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var reactionToEdit = await context.Reactions.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (reactionToEdit is null)
            return Result.Failure<ReactionResponse>(new Error("Reaction.NotFound"), HttpStatusCode.NotFound);

        reactionToEdit.Type = request.Type;

        await context.SaveChangesAsync(cancellationToken);

        var profile = await context.GetProfileAsync(reactionToEdit.UserId, cancellationToken);

        return Result.Success(reactionToEdit.MapToResponse(profile));
    }
}
