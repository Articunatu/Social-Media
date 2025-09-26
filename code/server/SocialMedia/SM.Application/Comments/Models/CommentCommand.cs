namespace SM.Application.Comments.Models;

public record CommentCommand(
    Guid Id,
    string Content,
    Guid UserId,
    DateTimeOffset TimeStamp,
    Guid ParentPostId);
