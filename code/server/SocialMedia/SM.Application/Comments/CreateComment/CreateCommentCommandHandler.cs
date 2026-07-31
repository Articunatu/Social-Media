using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Comments.Extensions;
using SM.Application.Comments.Models;
using SM.Application.Database;
using SM.Domain.Content;
using SM.Domain.Shared;
using System.Net;

namespace SM.Application.Comments.CreateComment;

internal class CreateCommentCommandHandler(IDbContextFactory<ApplicationDbContext> contextFactory) : ICommandHandler<CreateCommentCommand, CommentCommand>
{
    public async Task<Result<CommentCommand>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var postExists = await context.Posts.AnyAsync(p => p.Id == request.ParentPostId, cancellationToken);
        if (!postExists)
            return Result.Failure<CommentCommand>(new Error("Post.NotFound"), HttpStatusCode.NotFound);

        var authorExists = await context.Users.AnyAsync(u => u.Id == request.AuthorId, cancellationToken);
        if (!authorExists)
            return Result.Failure<CommentCommand>(new Error("User.NotFound"), HttpStatusCode.NotFound);

        if (request.ParentCommentId.HasValue)
        {
            var parentCommentExists = await context.Comments.AnyAsync(c => c.Id == request.ParentCommentId.Value && c.ParentPostId == request.ParentPostId, cancellationToken);
            if (!parentCommentExists)
                return Result.Failure<CommentCommand>(new Error("ParentComment.NotFound"), HttpStatusCode.NotFound);
        }

        var comment = Comment.Create(request.ParentPostId, request.Content, request.AuthorId, request.ParentCommentId);

        context.Comments.Add(comment);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(comment.MapToResponse());
    }
}
