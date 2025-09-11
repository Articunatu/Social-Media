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

        var reaction = await context.Reactions
            .AsSplitQuery()
            .Include(r => r.User)
            .Select(r => new Reaction(r.Id)
            {
                Type = r.Type,
                MessageId = r.MessageId,
                User = new User(r.User.Id, r.User.Tag, r.User.FirstName, r.User.LastName)
            })
            .FirstOrDefaultAsync(r => r.Id == reactionToAdd.Id, cancellationToken);

        if (reaction is null)
            return Result.Failure<ReactionResponse>(new Error("ReactionDisappeared"), HttpStatusCode.NotFound);

        return Result.Success(reaction.MapToResponse());
    }
}
