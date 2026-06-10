using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Behaviors;
using SM.Application.Database;
using SM.Domain.Shared;
using System.Net;

namespace SM.Application.Posts.DeletePost;

internal class DeletePostCommandHandler(IDbContextFactory<ApplicationDbContext> contextFactory, ILoggingBehaviour logging) : ICommandHandler<DeletePostCommand, PostResponse>
{
    public async Task<Result<PostResponse>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var postToDelete = await context.Posts
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        if (postToDelete is null)
            return Result.Failure<PostResponse>(new Error("Post.NotFound"), HttpStatusCode.NotFound);

        if (postToDelete.AuthorId != request.UserId)
            return Result.Failure<PostResponse>(new Error("Post.Forbidden"), HttpStatusCode.Forbidden);

        postToDelete.SoftDelete();

        await context.SaveChangesAsync(cancellationToken);
        logging.LogInformation($"Post with id {request.Id} from author with id {postToDelete.AuthorId} marked as deleted.");

        return Result.Success(postToDelete.MapToResponse());
    }
}
