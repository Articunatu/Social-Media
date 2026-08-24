using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Domain.Shared;
using System.Net;

namespace SM.Application.Reactions.RemoveReaction;

internal class RemoveReactionCommandHandler(
    IDbContextFactory<ContentDbContext> contentContextFactory,
    IDbContextFactory<IdentityDbContext> identityContextFactory,
    IDbContextFactory<MediaDbContext> mediaContextFactory)
    : ICommandHandler<RemoveReactionCommand, ReactionResponse>
{
    public async Task<Result<ReactionResponse>> Handle(RemoveReactionCommand request, CancellationToken cancellationToken)
    {
        await using var contentContext = await contentContextFactory.CreateDbContextAsync(cancellationToken);
        await using var identityContext = await identityContextFactory.CreateDbContextAsync(cancellationToken);
        await using var mediaContext = await mediaContextFactory.CreateDbContextAsync(cancellationToken);

        var reactionToRemove = await contentContext.Reactions
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (reactionToRemove is null)
            return Result.Failure<ReactionResponse>(new Error("NotFound"), HttpStatusCode.NotFound);

        var profile = await identityContext.GetProfileAsync(mediaContext, reactionToRemove.UserId, cancellationToken);

        contentContext.Reactions.Remove(reactionToRemove);

        await contentContext.SaveChangesAsync(cancellationToken);

        return Result.Success(reactionToRemove.MapToResponse(profile));
    }
}
