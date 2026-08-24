using System.Net;
using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Domain.Messaging;
using SM.Domain.Shared;

namespace SM.Application.Messaging.SendMessage;

internal sealed class SendMessageCommandHandler(
    IDbContextFactory<IdentityDbContext> identityContextFactory,
    IDbContextFactory<MessagingDbContext> messagingContextFactory)
    : ICommandHandler<SendMessageCommand, DirectMessageResponse>
{
    public async Task<Result<DirectMessageResponse>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Content))
            return Result.Failure<DirectMessageResponse>(new Error("Message.Empty"), HttpStatusCode.BadRequest);

        await using var identityContext = await identityContextFactory.CreateDbContextAsync(cancellationToken);
        var authorExists = await identityContext.Users
            .AsNoTracking()
            .AnyAsync(user => user.Id == request.AuthorId, cancellationToken);
        if (!authorExists)
            return Result.Failure<DirectMessageResponse>(new Error("User.NotFound"), HttpStatusCode.NotFound);

        await using var messagingContext = await messagingContextFactory.CreateDbContextAsync(cancellationToken);
        var conversationExists = await messagingContext.Conversations
            .AnyAsync(conversation => conversation.Id == request.ConversationId, cancellationToken);
        if (!conversationExists)
            return Result.Failure<DirectMessageResponse>(new Error("Conversation.NotFound"), HttpStatusCode.NotFound);

        var message = DirectMessage.Create(request.ConversationId, request.Content.Trim(), request.AuthorId);
        messagingContext.DirectMessages.Add(message);
        await messagingContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new DirectMessageResponse(
            message.Id,
            message.ConversationId,
            message.AuthorId,
            message.Content,
            message.TimeStamp));
    }
}