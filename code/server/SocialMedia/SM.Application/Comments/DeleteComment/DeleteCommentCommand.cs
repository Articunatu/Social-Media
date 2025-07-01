using SM.Application.Abstractions;

namespace SM.Application.Comments.DeleteComment
{
    public record DeleteCommentCommand(Guid Id) : ICommand<CommentCommand>;
}
