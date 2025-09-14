using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Domain.Reactions;
using SM.Domain.Shared;
using SM.Domain.Users;
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
            MessageId = request.MessageId
        };

        context.Reactions.Add(reactionToAdd);

        await context.SaveChangesAsync(cancellationToken);

        var reactionData = await context.Reactions
            .Where(r => r.Id == reactionToAdd.Id)
            .Select(r => new
            {
                r.Id,
                r.Type,
                r.MessageId,
                User = new
                {
                    r.User.Id,
                    r.User.Tag,
                    r.User.FirstName,
                    r.User.LastName
                }
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (reactionData is null)
            return Result.Failure<ReactionResponse>(
                new Error("ReactionDisappeared"), HttpStatusCode.NotFound);

        var reaction = new Reaction(reactionData.Id)
        {
            Type = reactionData.Type,
            MessageId = reactionData.MessageId,
            User = new User(
                reactionData.User.Id,
                reactionData.User.Tag,
                reactionData.User.FirstName,
                reactionData.User.LastName)
        };

        return Result.Success(reaction.MapToResponse());

    }
}
