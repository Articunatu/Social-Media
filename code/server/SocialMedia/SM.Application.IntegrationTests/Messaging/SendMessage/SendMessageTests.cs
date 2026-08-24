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

        var conversation = Conversation.Create();
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
}
