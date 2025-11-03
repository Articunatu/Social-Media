using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Domain.Reactions;
using SM.Domain.Shared;
using System.Net;

namespace SM.Application.Reactions.AddReaction;

internal class AddReactionCommandHandler(IDbContextFactory<ApplicationDbContext> contextFactory) : ICommandHandler<AddReactionCommand, ReactionResponse>
{
    public async Task<Result<ReactionResponse>> Handle(AddReactionCommand request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var reactionToAdd = new Reaction(Guid.CreateVersion7())
        {
            Type = request.Type,
            UserId = request.UserId,
            PostId = request.MessageId
        };

        context.Reactions.Add(reactionToAdd);

        await context.SaveChangesAsync(cancellationToken);

        var reaction = await context.Reactions.FirstOrDefaultAsync(r => r.Id == reactionToAdd.Id, cancellationToken);

        if (reaction is null)
            return Result.Failure<ReactionResponse>(
                new Error("ReactionDisappeared"), HttpStatusCode.NotFound);

        return Result.Success(reaction.MapToResponse());
    }
}
