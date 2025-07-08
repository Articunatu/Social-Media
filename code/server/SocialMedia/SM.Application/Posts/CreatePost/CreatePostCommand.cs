using SM.Application.Abstractions;

namespace SM.Application.Posts.CreatePost;

public record CreatePostCommand(string Content, Guid AuthorId) : ICommand<PostResponse>;
