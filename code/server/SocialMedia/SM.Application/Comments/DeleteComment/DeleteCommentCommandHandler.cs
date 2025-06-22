using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Domain.Shared;

namespace SM.Application.Comments.DeleteComment;

internal class DeleteCommentCommandHandler(IDbContextFactory<ApplicationDbContext> contextFactory)
    : ICommandHandler<DeleteCommentCommand, CommentResponse>
{
    public async Task<Result<CommentResponse>> Handle(DeleteCommentCommand request, CancellationToken ct)
    {
        await using var context = await contextFactory.CreateDbContextAsync(ct);

        var commentToDelete = await context.Comments.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken: ct);

        if (commentToDelete is null)
            return Result.Failure<CommentResponse>(new Error("Comment.NotFound"));

        context.Comments.Remove(commentToDelete);

        await context.SaveChangesAsync(ct);

        return Result.Success(commentToDelete.MapToResponse());
    }
}
