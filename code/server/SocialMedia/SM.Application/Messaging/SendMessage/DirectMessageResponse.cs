namespace SM.Application.Messaging.SendMessage;

public sealed record DirectMessageResponse(
    Guid Id,
    Guid ConversationId,
    Guid AuthorId,
    string Content,
    DateTimeOffset TimeStamp);