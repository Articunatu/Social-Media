using Microsoft.EntityFrameworkCore;
using SM.Application.Comments.DeleteComment;
using SM.Application.IntegrationTests.IntegrationAbstractions;
using SM.Domain.Content;
using SM.Domain.Users;
using System.Net;

namespace SM.Application.IntegrationTests.Comments.DeleteComment;

public class DeleteCommentTests(IntegrationTestFixture fixture) : BaseIntegrationTest(fixture)
{
    [Fact]
    public async Task Handle_ShouldReturnForbiddenAndKeepComment_WhenUserIsNotAuthor()
    {
        var postAuthor = User.Create(new UserDto("comment_post_author", "Post", "Owner", "comment.post.owner@example.com"));
        var commentAuthor = User.Create(new UserDto("comment_author", "Comment", "Author", "comment.author@example.com"));
        var otherUser = User.Create(new UserDto("comment_intruder", "Comment", "Intruder", "comment.intruder@example.com"));
        var post = Post.Create("A post with comments.", postAuthor.Id);
        var comment = Comment.Create(post.Id, "Only the comment author can delete this.", commentAuthor.Id);

        await Users.AddAsync(postAuthor, commentAuthor, otherUser);
        ContentDbContext.Posts.Add(post);
        ContentDbContext.Comments.Add(comment);
        await ContentDbContext.SaveChangesAsync();

        var result = await Sender.Send(new DeleteCommentCommand(comment.Id, otherUser.Id));

        var commentFromDb = await ContentDbContext.Comments
            .AsNoTracking()
            .FirstAsync(c => c.Id == comment.Id);

        using (new AssertionScope())
        {
            result.IsFailure.Should().BeTrue();
            result.Status.Should().Be(HttpStatusCode.Forbidden);
            result.Error.Header.Should().Be("Comment.Forbidden");
            commentFromDb.IsDeleted.Should().BeFalse();
            commentFromDb.TimeOfDelete.Should().BeNull();
        }
    }
}
