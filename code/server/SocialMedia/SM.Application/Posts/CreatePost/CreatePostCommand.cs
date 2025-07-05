using SM.Application.Abstractions;

namespace SM.Application.Posts.CreatePost;

public record CreatePostCommand : ICommand<PostResponse>
{
    public string Content { get; set; } = string.Empty; 
    public Guid AuthorId { get; set; }
}
