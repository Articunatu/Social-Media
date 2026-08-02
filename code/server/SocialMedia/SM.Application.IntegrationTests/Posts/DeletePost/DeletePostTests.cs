using Microsoft.EntityFrameworkCore;
using SM.Application.IntegrationTests.IntegrationAbstractions;
using SM.Application.Posts.DeletePost;
using SM.Domain.Content;
using SM.Domain.Users;
using System.Net;

namespace SM.Application.IntegrationTests.Posts.DeletePost;

public class DeletePostTests(IntegrationTestFixture fixture) : BaseIntegrationTest(fixture)
{
    [Fact]
    public async Task Handle_ShouldReturnForbiddenAndKeepPost_WhenUserIsNotAuthor()
    {
        var author = User.Create(new UserDto("post_author", "Post", "Author", "post.author@example.com"));
        var otherUser = User.Create(new UserDto("post_intruder", "Post", "Intruder", "post.intruder@example.com"));
        var post = Post.Create("Only the author can delete this.", author.Id);

        await Users.AddAsync(author, otherUser);
        ContentDbContext.Posts.Add(post);
        await ContentDbContext.SaveChangesAsync();

        var result = await Sender.Send(new DeletePostCommand(post.Id, otherUser.Id));

        var postFromDb = await ContentDbContext.Posts
            .AsNoTracking()
            .FirstAsync(p => p.Id == post.Id);

        using (new AssertionScope())
        {
            result.IsFailure.Should().BeTrue();
            result.Status.Should().Be(HttpStatusCode.Forbidden);
            result.Error.Header.Should().Be("Post.Forbidden");
            postFromDb.IsDeleted.Should().BeFalse();
            postFromDb.TimeOfDelete.Should().BeNull();
        }
    }
}
