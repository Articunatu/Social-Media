
namespace SM.Application.Comments;

public record CommentResponse(Guid Id, string Content, Guid UserId, DateTimeOffset TimeStamp, Guid ParentPostId);
