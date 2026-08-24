using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Domain.Content;
using SM.Domain.Shared;
using System.Net;

namespace SM.Application.Reactions.AddReaction;

internal class AddReactionCommandHandler(IDbContextFactory<IdentityDbContext> identityContextFactory, IDbContextFactory<ContentDbContext> contentContextFactory, IDbContextFactory<MediaDbContext> mediaContextFactory) : ICommandHandler<AddReactionCommand, ReactionResponse>
{
    public async Task<Result<ReactionResponse>> Handle(AddReactionCommand request, CancellationToken cancellationToken)
    {
        await using var identityContext = await identityContextFactory.CreateDbContextAsync(cancellationToken);
        await using var contentContext = await contentContextFactory.CreateDbContextAsync(cancellationToken);
        await using var mediaContext = await mediaContextFactory.CreateDbContextAsync(cancellationToken);

        var userExists = await identityContext.Users.AnyAsync(u => u.Id == request.UserId, cancellationToken);
        if (!userExists)
            return Result.Failure<ReactionResponse>(new Error("User.NotFound", "User not found"), HttpStatusCode.NotFound);

        var postExists = await contentContext.Posts.AnyAsync(p => p.Id == request.MessageId, cancellationToken);
        var commentExists = await contentContext.Comments.AnyAsync(c => c.Id == request.MessageId, cancellationToken);

        if (!postExists && !commentExists)
            return Result.Failure<ReactionResponse>(new Error("Target.NotFound", "The target post or comment was not found"), HttpStatusCode.NotFound);

        if (postExists && commentExists)
            return Result.Failure<ReactionResponse>(new Error("Target.Ambiguous", "MessageId matches both a post and a comment. Cannot determine reaction target."), HttpStatusCode.BadRequest);

        Guid? postId = postExists ? request.MessageId : null;
        Guid? commentId = commentExists ? request.MessageId : null;

        var existingReaction = await contentContext.Reactions.FirstOrDefaultAsync(r =>
            r.UserId == request.UserId && r.PostId == postId && r.CommentId == commentId, cancellationToken);

        if (existingReaction != null)
            return Result.Failure<ReactionResponse>(new Error("Reaction.AlreadyExists", "User already reacted to this target"), HttpStatusCode.BadRequest);

        var reactionToAdd = new Reaction(Guid.CreateVersion7())
        {
            Type = request.Type,
            UserId = request.UserId,
            PostId = postId,
            CommentId = commentId
        };

        contentContext.Reactions.Add(reactionToAdd);

        await contentContext.SaveChangesAsync(cancellationToken);

        var reaction = await contentContext.Reactions
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == reactionToAdd.Id, cancellationToken);

        if (reaction is null)
            return Result.Failure<ReactionResponse>(new Error("ReactionDisappeared"), HttpStatusCode.NotFound);

        var profile = await identityContext.GetProfileAsync(mediaContext, reaction.UserId, cancellationToken);

        return Result.Success(reaction.MapToResponse(profile));
    }
}
