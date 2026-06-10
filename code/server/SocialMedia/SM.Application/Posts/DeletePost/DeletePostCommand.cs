using SM.Application.Abstractions;

namespace SM.Application.Posts.DeletePost;

public record DeletePostCommand(Guid Id, Guid UserId) : ICommand<PostResponse>;
