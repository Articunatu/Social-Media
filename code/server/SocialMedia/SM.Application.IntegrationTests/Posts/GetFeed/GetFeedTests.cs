using Microsoft.EntityFrameworkCore;
using SM.Application.IntegrationTests.IntegrationAbstractions;
using SM.Application.Posts.CreatePost;
using SM.Application.Posts.GetFeed;
using SM.Application.Shared.Models;
using SM.Application.Users.Follow;
using SM.Domain.Users;

namespace SM.Application.IntegrationTests.Posts.GetFeed;

public class GetFeedTests(IntegrationTestFixture fixture) : BaseIntegrationTest(fixture)
{
    [Fact]
    public async Task Handle_ShouldProjectNewPostToFollowersFeed()
    {
        var follower = User.Create(new UserDto("feed_follower", "Feed", "Follower", "feed.follower@example.com"));
        var author = User.Create(new UserDto("feed_author", "Feed", "Author", "feed.author@example.com"));
        await Users.AddAsync(follower, author);

        var followResult = await Sender.Send(new FollowCommand(follower.Id, author.Id));
        var postResult = await Sender.Send(new CreatePostCommand("A projected post", author.Id));

        var feedItem = await FeedDbContext.FeedItems
            .SingleOrDefaultAsync(item => item.RecipientId == follower.Id && item.PostId == postResult.Value!.Id);
        var feedResult = await Sender.Send(new GetFeedQuery(follower.Id, new PageFilter()));

        followResult.IsSuccess.Should().BeTrue();
        postResult.IsSuccess.Should().BeTrue();
        feedItem.Should().NotBeNull();
        feedResult.IsSuccess.Should().BeTrue();
        feedResult.Value!.Values.Should().ContainSingle(item => item.Post.PostId == postResult.Value!.Id);
    }

    [Fact]
    public async Task Handle_ShouldBackfillExistingPostsWhenFollowingAuthor()
    {
        var follower = User.Create(new UserDto("backfill_follower", "Backfill", "Follower", "backfill.follower@example.com"));
        var author = User.Create(new UserDto("backfill_author", "Backfill", "Author", "backfill.author@example.com"));
        await Users.AddAsync(follower, author);

        var postResult = await Sender.Send(new CreatePostCommand("An existing post", author.Id));
        var followResult = await Sender.Send(new FollowCommand(follower.Id, author.Id));

        var feedResult = await Sender.Send(new GetFeedQuery(follower.Id, new PageFilter()));

        postResult.IsSuccess.Should().BeTrue();
        followResult.IsSuccess.Should().BeTrue();
        feedResult.IsSuccess.Should().BeTrue();
        feedResult.Value!.Values.Should().ContainSingle(item => item.Post.PostId == postResult.Value!.Id);
    }
}