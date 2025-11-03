using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Comments.Extensions;
using SM.Application.Comments.Models;
using SM.Application.Database;
using SM.Domain.Shared;
using System.Net;

namespace SM.Application.Comments.DeleteComment;

internal class DeleteCommentCommandHandler(IDbContextFactory<ApplicationDbContext> contextFactory)
    : ICommandHandler<DeleteCommentCommand, CommentCommand>
{
    public async Task<Result<CommentCommand>> Handle(DeleteCommentCommand request, CancellationToken ct)
    {
        await using var context = await contextFactory.CreateDbContextAsync(ct);

        var commentToDelete = await context.Comments
            .Include(c => c.Replies)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken: ct);

        if (commentToDelete is null)
            return Result.Failure<CommentCommand>(new Error("Comment.NotFound"), HttpStatusCode.NotFound);

        commentToDelete.IsDeleted = true;
        commentToDelete.TimeOfDelete = DateTime.UtcNow;

        foreach (var reply in commentToDelete.Replies)
        {
            reply.IsDeleted = true;
            reply.TimeOfDelete = DateTime.UtcNow;
        }

        await context.SaveChangesAsync(ct);

        return Result.Success(commentToDelete.MapToResponse());
    }
}
