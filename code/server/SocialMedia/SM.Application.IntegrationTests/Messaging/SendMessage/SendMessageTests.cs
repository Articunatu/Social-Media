using Microsoft.EntityFrameworkCore;
using SM.Application.IntegrationTests.IntegrationAbstractions;
using SM.Application.Messaging.SendMessage;
using SM.Application.Shared.Models;
using SM.Domain.Messaging;
using SM.Domain.Users;

namespace SM.Application.IntegrationTests.Messaging.SendMessage;

public sealed class SendMessageTests(IntegrationTestFixture fixture) : BaseIntegrationTest(fixture)
{
    [Fact]
    public async Task Handle_ShouldPersistMessageForExistingUserAndConversation()
    {
        var author = User.Create(new UserDto(
            "message_author",
            "Message",
            "Author",
            "message.author@example.com"));
        await Users.AddAsync(author);

        var conversation = Conversation.Create([author.Id]);
        MessagingDbContext.Conversations.Add(conversation);
        await MessagingDbContext.SaveChangesAsync();

        var result = await Sender.Send(new SendMessageCommand(
            conversation.Id,
            author.Id,
            "  Hello from messaging  "));

        var persistedMessage = await MessagingDbContext.DirectMessages
            .SingleAsync(message => message.ConversationId == conversation.Id);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Content.Should().Be("Hello from messaging");
        persistedMessage.AuthorId.Should().Be(author.Id);
        persistedMessage.Content.Should().Be("Hello from messaging");
    }

    [Fact]
    public async Task Handle_ShouldRejectMessageFromNonParticipant()
    {
        var author = User.Create(new UserDto(
            "message_author",
            "Message",
            "Author",
            "message.author@example.com"));
        var participant = User.Create(new UserDto(
            "conversation_participant",
            "Conversation",
            "Participant",
            "conversation.participant@example.com"));
        await Users.AddAsync(author);
        await Users.AddAsync(participant);

        var conversation = Conversation.Create([participant.Id]);
        MessagingDbContext.Conversations.Add(conversation);
        await MessagingDbContext.SaveChangesAsync();

        var result = await Sender.Send(new SendMessageCommand(
            conversation.Id,
            author.Id,
            "Not allowed"));

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(System.Net.HttpStatusCode.Forbidden);
        result.Error!.Header.Should().Be("Conversation.NotParticipant");
        (await MessagingDbContext.DirectMessages.CountAsync()).Should().Be(0);
    }
}
