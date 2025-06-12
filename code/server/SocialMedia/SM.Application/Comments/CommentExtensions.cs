using SM.Domain.Messages;

namespace SM.Application.Comments;

public static class CommentExtensions
{
    public static CommentResponse MapToResponse(this Comment comment)
    {
        return new CommentResponse(comment.Id, comment.Content, comment.AuthorId, comment.TimeStamp, comment.ParentPostId);
    }
}
