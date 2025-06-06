using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Domain.Shared;

namespace SM.Application.Posts.DeletePost;

internal class DeletePostCommandHandler(ApplicationDbContext context) : ICommandHandler<DeletePostCommand, PostResponse>
{
    public async Task<Result<PostResponse>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var postToDelete = await context.Posts.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        if (postToDelete is null)
            return Result.Failure<PostResponse>(new Error("Post.NotFound"));

        context.Posts.Remove(postToDelete);

        await context.SaveChangesAsync(cancellationToken);

        var response = postToDelete.MapToResponse();

        return Result.Success(response);
    }
}
