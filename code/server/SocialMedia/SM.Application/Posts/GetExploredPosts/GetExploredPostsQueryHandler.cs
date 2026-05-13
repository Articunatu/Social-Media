using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Posts.GetProfilePosts;
using SM.Application.Shared.Extensions;
using SM.Application.Shared.Models;
using SM.Domain.Shared;
using System.Net;

namespace SM.Application.Posts.GetExploredPosts;

internal class GetExploredPostsQueryHandler(IDbContextFactory<ApplicationDbContext> contextFactory) : IQueryHandler<GetExploredPostsQuery, ProfileFeedResponse>
{
    public async Task<Result<ProfileFeedResponse>> Handle(GetExploredPostsQuery request, CancellationToken ct)
    {
        await using var context = await contextFactory.CreateDbContextAsync(ct);

        var posts = await context.Posts
            .Select(p => new
            {
                p.Id,
                p.Content,
                p.TimeStamp,
                CommentsCount = p.Comments != null ? p.Comments.Count() : 0,
                ReactionCounts = p.Reactions.GroupBy(r => r.Type)
                    .Select(rt => new { Type = rt.Key, Count = rt.Count() })
            })
            .ToPagedFeed(new PageFilter());

        var mapped = posts.Values.Select(p => new ProfilePostDto
        {
            PostId = p.Id,
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
