using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Application.Shared.Models;
using SM.Domain.Shared;

namespace SM.Application.Reactions.GetReactionsByPost;

internal class GetReactionsByPostQueryHandler(IDbContextFactory<ApplicationDbContext> contextFactory)
    : IQueryHandler<GetReactionsByPostQuery, PagedFeed<ReactionResponse>>
{
    public async Task<Result<PagedFeed<ReactionResponse>>> Handle(GetReactionsByPostQuery request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var pagedReactions = await context.Reactions
            .AsNoTracking()
            .Where(r => r.PostId == request.PostId && (!request.Type.HasValue || r.Type == request.Type.Value))
            .ToPagedFeed(request.Filter);

        var profiles = await context.GetProfileLookupAsync(pagedReactions.Values.Select(r => r.UserId), cancellationToken);

        return Result.Success(new PagedFeed<ReactionResponse>
        {
            Index = pagedReactions.Index,
            Order = pagedReactions.Order,
            SearchText = pagedReactions.SearchText,
            Values = pagedReactions.Values.Select(r => r.MapToResponse(
                profiles.TryGetValue(r.UserId, out var profile)
                    ? profile
                    : new ProfileInfo(r.UserId, string.Empty, string.Empty, null)))
        });
    }
}
