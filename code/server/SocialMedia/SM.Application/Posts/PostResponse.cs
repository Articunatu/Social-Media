namespace SM.Application.Posts;

public record PostResponse(Guid Id, string Content, Guid UserId, DateTimeOffset TimeStamp);
