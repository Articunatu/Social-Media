
namespace SM.Application.Comments;

public record CommentCommand(
    Guid Id, 
    string Content, 
    Guid UserId, 
    DateTimeOffset TimeStamp, 
    Guid ParentPostId);
