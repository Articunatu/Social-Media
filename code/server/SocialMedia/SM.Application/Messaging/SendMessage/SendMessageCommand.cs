using SM.Application.Abstractions;

namespace SM.Application.Messaging.SendMessage;

public record SendMessageCommand(Guid ConversationId, Guid AuthorId, string Content) : ICommand<DirectMessageResponse>;