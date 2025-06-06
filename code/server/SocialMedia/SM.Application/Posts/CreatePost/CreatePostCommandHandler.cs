using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Domain.Messages;
using SM.Domain.Shared;

namespace SM.Application.Posts.CreatePost;

internal class CreatePostCommandHandler(ApplicationDbContext context) : ICommandHandler<CreatePostCommand, PostResponse>
{
    public async Task<Result<PostResponse>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var post = Post.Create(request.Content, request.AuthorId);

        context.Posts.Add(post);

        await context.SaveChangesAsync(cancellationToken);

        var response = post.MapToResponse();

        return Result.Success(response);
    }
}
