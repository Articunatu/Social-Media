using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Comments.Extensions;
using SM.Application.Comments.Models;
using SM.Application.Database;
using SM.Domain.Messages;
using SM.Domain.Shared;

namespace SM.Application.Comments.CreateComment;

internal class CreateCommentCommandHandler(IDbContextFactory<ApplicationDbContext> contextFactory) : ICommandHandler<CreateCommentCommand, CommentCommand>
{
    public async Task<Result<CommentCommand>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var comment = Comment.Create(request.ParentPostId, request.Content, request.AuthorId);

        context.Comments.Add(comment);

        await context.SaveChangesAsync(cancellationToken);

        var response = comment.MapToResponse();

        return Result.Success(response);
    }
}
