using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Photos;
using SM.Application.Shared.Extensions;
using SM.Application.Shared.Models;
using SM.Domain.Photos;
using SM.Domain.Shared;
using System.Net;

namespace SM.Application.Posts.GetPostById;

internal class GetPostByIdQueryHandler(
    IDbContextFactory<ContentDbContext> contentContextFactory,
    IDbContextFactory<IdentityDbContext> identityContextFactory,
    IDbContextFactory<MediaDbContext> mediaContextFactory)
    : IQueryHandler<GetPostByIdQuery, PostDetailsResponse>
{
    public async Task<Result<PostDetailsResponse>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        await using var contentContext = await contentContextFactory.CreateDbContextAsync(cancellationToken);
        await using var identityContext = await identityContextFactory.CreateDbContextAsync(cancellationToken);
        await using var mediaContext = await mediaContextFactory.CreateDbContextAsync(cancellationToken);

        if (request.Profile is null)
        {
            Guid authorId = await contentContext.Posts
                .AsNoTracking()
                .Where(p => p.Id == request.Id && !p.IsDeleted)
                .Select(p => p.AuthorId)
                .FirstOrDefaultAsync(cancellationToken);

            var userProfile = await identityContext.Users
                .AsNoTracking()
                .Where(u => u.Id == authorId)
                .FirstOrDefaultAsync(cancellationToken);

            if (userProfile is null)
                return Result.Failure<PostDetailsResponse>(new Error("User.NotFound"), HttpStatusCode.NotFound);

            var profilePhoto = await mediaContext.Photos
                .AsNoTracking()
                .Where(p => p.UserId == authorId && p.Type == PhotoType.Profile)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => p.MapToResponse())
                .FirstOrDefaultAsync(cancellationToken);

            request.Profile = userProfile.MapToProfile(profilePhoto);
        }

        request.Post ??= await contentContext.Posts
            .AsNoTracking()
            .Where(p => p.Id == request.Id && !p.IsDeleted)
            .Select(p => new ProfilePostDto
            {
                PostId = p.Id,
                Content = p.Content,
                TimeStamp = p.TimeStamp,
                CommentsCount = p.Comments.Where(c => !c.IsDeleted).Count(),
                ReactionCounts = p.Reactions
                    .GroupBy(r => r.Type)
                    .Select(rt => new ReactionCount(rt.Key, rt.Count()))
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (request.Post is null)
            return Result.Failure<PostDetailsResponse>(new Error("Post.NotFound"), HttpStatusCode.NotFound);

        var commentsQuery = contentContext.Comments
            .AsNoTracking()
            .Where(c => c.ParentPostId == request.Id && c.ParentCommentId == null && !c.IsDeleted)
            .OrderByDescending(c => c.TimeStamp)
            .Select(p => new ProfilePostDto
            {
                PostId = p.Id,
                Content = p.Content,
                TimeStamp = p.TimeStamp,
                CommentsCount = p.Replies.Where(r => !r.IsDeleted).Count(),
                ReactionCounts = p.Reactions
                    .GroupBy(r => r.Type)
                    .Select(rt => new ReactionCount(rt.Key, rt.Count()))
            });

        var pagedComments = await commentsQuery.ToPagedFeed(request.Filter);

        return Result.Success(new PostDetailsResponse(request.Profile, request.Post, pagedComments));
    }
}
