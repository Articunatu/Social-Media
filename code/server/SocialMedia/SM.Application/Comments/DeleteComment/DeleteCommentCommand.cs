using SM.Application.Abstractions;
using SM.Application.Comments.Models;

namespace SM.Application.Comments.DeleteComment
{
    public record DeleteCommentCommand(Guid Id, Guid UserId) : ICommand<CommentCommand>;
}
