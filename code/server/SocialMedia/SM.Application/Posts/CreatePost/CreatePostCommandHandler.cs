using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Domain.Content;
using SM.Domain.Shared;
using System.Net;

namespace SM.Application.Posts.CreatePost;

internal class CreatePostCommandHandler(IDbContextFactory<IdentityDbContext> identityContextFactory, IDbContextFactory<ContentDbContext> contentContextFactory) 
    : ICommandHandler<CreatePostCommand, PostResponse>
{
    public async Task<Result<PostResponse>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        await using var identityContext = await identityContextFactory.CreateDbContextAsync(cancellationToken);
        await using var contentContext = await contentContextFactory.CreateDbContextAsync(cancellationToken);

        var authorExists = await identityContext.Users
            .AsNoTracking()
            .AnyAsync(u => u.Id == request.AuthorId, cancellationToken);
        if (!authorExists)
            return Result.Failure<PostResponse>(new Error("User.NotFound"), HttpStatusCode.NotFound);

        var post = Post.Create(request.Content, request.AuthorId);

        contentContext.Posts.Add(post);
        await contentContext.SaveChangesAsync(cancellationToken);

        return Result.Success(post.MapToResponse());
    }
}
