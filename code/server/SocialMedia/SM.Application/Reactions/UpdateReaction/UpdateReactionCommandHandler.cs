using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Domain.Shared;
using System.Net;

namespace SM.Application.Reactions.UpdateReaction;

internal class UpdateReactionCommandHandler(
    IDbContextFactory<ContentDbContext> contentContextFactory,
    IDbContextFactory<IdentityDbContext> identityContextFactory,
    IDbContextFactory<MediaDbContext> mediaContextFactory)
    : ICommandHandler<UpdateReactionCommand, ReactionResponse>
{
    public async Task<Result<ReactionResponse>> Handle(UpdateReactionCommand request, CancellationToken cancellationToken)
    {
        await using var contentContext = await contentContextFactory.CreateDbContextAsync(cancellationToken);
        await using var identityContext = await identityContextFactory.CreateDbContextAsync(cancellationToken);
        await using var mediaContext = await mediaContextFactory.CreateDbContextAsync(cancellationToken);

        var reactionToEdit = await contentContext.Reactions.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (reactionToEdit is null)
            return Result.Failure<ReactionResponse>(new Error("Reaction.NotFound"), HttpStatusCode.NotFound);

        reactionToEdit.Type = request.Type;

        await contentContext.SaveChangesAsync(cancellationToken);

        var profile = await identityContext.GetProfileAsync(mediaContext, reactionToEdit.UserId, cancellationToken);

        return Result.Success(reactionToEdit.MapToResponse(profile));
    }
}
