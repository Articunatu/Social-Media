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

        var userExists = await context.Users.AnyAsync(u => u.Id == request.UserId, cancellationToken);
        if (!userExists)
            return Result.Failure<ReactionResponse>(new Error("User.NotFound", "User not found"), HttpStatusCode.NotFound);

        var postExists = await context.Posts.AnyAsync(p => p.Id == request.MessageId, cancellationToken);
        var commentExists = await context.Comments.AnyAsync(c => c.Id == request.MessageId, cancellationToken);

        if (!postExists && !commentExists)
            return Result.Failure<ReactionResponse>(new Error("Target.NotFound", "The target post or comment was not found"), HttpStatusCode.NotFound);

        Guid postId = postExists ? request.MessageId : Guid.Empty;
        Guid commentId = commentExists ? request.MessageId : Guid.Empty;

        var existingReaction = await context.Reactions.FirstOrDefaultAsync(r =>
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

        context.Reactions.Add(reactionToAdd);

        await context.SaveChangesAsync(cancellationToken);

        var reaction = await context.Reactions
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == reactionToAdd.Id, cancellationToken);

        if (reaction is null)
            return Result.Failure<ReactionResponse>(new Error("ReactionDisappeared"), HttpStatusCode.NotFound);

        return Result.Success(reaction.MapToResponse());
    }
}
