using FluentAssertions;
using SM.Domain.Content;
using SM.Domain.Content.Events;
using SM.Domain.Messaging;
using SM.Domain.Messaging.Events;

namespace SM.Domain.UnitTests;

public class ContentDomainEventTests
{
    [Fact]
    public void CreatePost_RaisesPostCreatedEvent()
    {
        var authorId = Guid.NewGuid();

        var post = Post.Create("A post", authorId);

        var domainEvent = post.GetDomainEvents().Should().ContainSingle().Which;
        domainEvent.Should().BeOfType<PostCreatedDomainEvent>().Which.Should().BeEquivalentTo(
            new PostCreatedDomainEvent(post.Id, authorId, post.TimeStamp));
    }

    [Fact]
    public void CreateComment_RaisesCommentAddedEvent()
    {
        var authorId = Guid.NewGuid();
        var postId = Guid.NewGuid();

        var comment = Comment.Create(postId, "A comment", authorId);

        var domainEvent = comment.GetDomainEvents().Should().ContainSingle().Which;
        domainEvent.Should().BeOfType<CommentAddedDomainEvent>().Which.Should().BeEquivalentTo(
            new CommentAddedDomainEvent(comment.Id, postId, authorId, comment.TimeStamp));
    }

    [Fact]
    public void CreateDirectMessage_RaisesMessageCreatedEvent()
    {
        var authorId = Guid.NewGuid();
        var conversationId = Guid.NewGuid();

        var message = DirectMessage.Create(conversationId, "A message", authorId);

        var domainEvent = message.GetDomainEvents().Should().ContainSingle().Which;
        domainEvent.Should().BeOfType<MessageCreatedDomainEvent>().Which.Should().BeEquivalentTo(
            new MessageCreatedDomainEvent(message.Id, conversationId, authorId, message.TimeStamp));
    }
}