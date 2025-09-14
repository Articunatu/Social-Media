using SM.Domain.Shared;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Models;
using SM.Application.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace SM.Application.Posts.GetProfilePosts;

internal class GetProfilePostsQueryHandler(IDbContextFactory<ApplicationDbContext> contextFactory)
    : IQueryHandler<GetProfilePostsQuery, ProfileFeedResponse>
{
    public async Task<Result<ProfileFeedResponse>> Handle(GetProfilePostsQuery request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var posts = await context.Posts
            .Where(p => p.AuthorId == request.UserId)
            .Select(p => new
            {
                p.Content,
                p.TimeStamp,
                CommentsCount = p.Comments != null ? p.Comments.Count() : 0,
                ReactionCounts = p.Reactions != null
                    ? p.Reactions.GroupBy(r => r.Type)
                        .Select(rt => new { Type = rt.Key, Count = rt.Count() })
                    : null
            })
            .ToPagedFeed(request.Filter);

        var mapped = posts.Values.Select(p => new ProfilePostDto
        {
            Content = p.Content,
            TimeStamp = p.TimeStamp,
            CommentsCount = p.CommentsCount,
            ReactionCounts = p.ReactionCounts != null
                ? p.ReactionCounts.Select(rc => new ReactionCount(rc.Type, rc.Count)).ToList()
                : []
        }).ToArray();

        if (mapped.Length == 0)
        {
            return Result.Failure<ProfileFeedResponse>(
                new Error("This user hasn't posted anything"), HttpStatusCode.NoContent);
        }

        return Result.Success(new ProfileFeedResponse(new PagedFeed<ProfilePostDto>
        {
            Index = posts.Index,
            Order = posts.Order,
            Values = mapped
        }));
    }
}

