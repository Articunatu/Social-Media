using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Application.Shared.Models;
using SM.Domain.Shared;
using System.Net;

namespace SM.Application.Posts.GetPostById;

internal class GetPostByIdQueryHandler(IDbContextFactory<ApplicationDbContext> contextFactory) : IQueryHandler<GetPostByIdQuery, PostDetailsResponse>
{
    public async Task<Result<PostDetailsResponse>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        if (request.Profile is null)
        {
            Guid authorId = await context.Posts
                .AsNoTracking()
                .Where(p => p.Id == request.Id && !p.IsDeleted)
                .Select(p => p.AuthorId)
                .FirstOrDefaultAsync(cancellationToken);

            var userProfile = await context.Users
                .AsNoTracking()
                .Where(u => u.Id == authorId)
                .Select(u => u.MapToProfile())
                .FirstOrDefaultAsync(cancellationToken);

            if (userProfile is null)
                return Result.Failure<PostDetailsResponse>(new Error("User.NotFound"), HttpStatusCode.NotFound);

            request.Profile = userProfile;
        }

        request.Post ??= await context.Posts
            .AsNoTracking()
            .Where(p => p.Id == request.Id && !p.IsDeleted)
            .Select(p => new ProfilePostDto
            {
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

        var commentsQuery = context.Comments
            .AsNoTracking()
            .Where(c => c.ParentPostId == request.Id && c.ParentCommentId == null && !c.IsDeleted)
            .OrderByDescending(c => c.TimeStamp)
            .Select(p => new ProfilePostDto
            {
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
