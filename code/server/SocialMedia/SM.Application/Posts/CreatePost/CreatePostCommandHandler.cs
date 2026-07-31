using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Domain.Content;
using SM.Domain.Shared;
using System.Net;

namespace SM.Application.Posts.CreatePost;

internal class CreatePostCommandHandler(IDbContextFactory<ApplicationDbContext> contextFactory) 
    : ICommandHandler<CreatePostCommand, PostResponse>
{
    public async Task<Result<PostResponse>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var authorExists = await context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Id == request.AuthorId, cancellationToken);
        if (!authorExists)
            return Result.Failure<PostResponse>(new Error("User.NotFound"), HttpStatusCode.NotFound);

        var post = Post.Create(request.Content, request.AuthorId);

        context.Posts.Add(post);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(post.MapToResponse());
    }
}
