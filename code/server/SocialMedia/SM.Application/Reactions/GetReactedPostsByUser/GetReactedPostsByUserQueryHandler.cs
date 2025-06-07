using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Models;
using SM.Domain.Shared;

namespace SM.Application.Reactions.GetReactedPostsByUser;

internal class GetReactedPostsByUserQueryHandler(ApplicationDbContext context)
    : IQueryHandler<GetReactedPostsByUserQuery, PagedFeed<ProfilePostDto>>
{
    public async Task<Result<PagedFeed<ProfilePostDto>>> Handle(GetReactedPostsByUserQuery request, CancellationToken cancellationToken)
    {
        var reactedPosts = await context.Reactions
            .Where(r => r.UserId == request.UserId)
            .Include(r => r.Message)
            .ThenInclude(m => m.Author)
            .Select(r => new ReactedProfilePost(r.Type)
            {
                Content = r.Message.Content,
                TimeStamp = r.Message.TimeStamp,
                ReactionCounts = r.Message.Reactions,
                RepliesCount
            })
    }
}
