using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Domain.Reactions;
using SM.Domain.Shared;

namespace SM.Application.Reactions.AddReaction;

internal class AddReactionCommandHandler(ApplicationDbContext context) : ICommandHandler<AddReactionCommand, ReactionResponse>
{
    public async Task<Result<ReactionResponse>> Handle(AddReactionCommand request, CancellationToken cancellationToken)
    {
        var reactionToAdd = new Reaction(Guid.CreateVersion7())
        {
            Type = request.Type,
            UserId = request.UserId,
            MessageId = request.MessageId
        };

        context.Reactions.Add(reactionToAdd);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(reactionToAdd.MapToResponse());
    }
}
