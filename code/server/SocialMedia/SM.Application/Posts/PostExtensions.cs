using SM.Domain.Messages;

namespace SM.Application.Posts;

public static class PostExtensions
{
    public static PostResponse MapToResponse(this Post post)
    {
        return new PostResponse(post.Id, post.Content, post.AuthorId, post.TimeStamp);
    }
}
