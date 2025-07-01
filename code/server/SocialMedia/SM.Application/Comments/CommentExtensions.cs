using SM.Domain.Messages;

namespace SM.Application.Comments;

public static class CommentExtensions
{
    public static CommentCommand MapToResponse(this Comment comment)
    {
        return new CommentCommand(comment.Id, comment.Content, comment.AuthorId, comment.TimeStamp, comment.ParentPostId);
    }
}
