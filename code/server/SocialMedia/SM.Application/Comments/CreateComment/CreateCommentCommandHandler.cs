using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Domain.Messages;
using SM.Domain.Shared;

namespace SM.Application.Comments.CreateComment;

internal class CreateCommentCommandHandler(ApplicationDbContext context) : ICommandHandler<CreateCommentCommand, CommentCommand>
{
    public async Task<Result<CommentCommand>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = Comment.Create(request.ParentPostId, request.Content, DateTime.Now, request.AuthorId);

        context.Comments.Add(comment);

        await context.SaveChangesAsync(cancellationToken);

        var response = comment.MapToResponse();

        return Result.Success(response);
    }
}
