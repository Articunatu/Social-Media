using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Application.Shared.Models;
using SM.Domain.Shared;

namespace SM.Application.Reactions.GetReactionsByPost;

internal class GetReactionsByPostQueryHandler(
    IDbContextFactory<ContentDbContext> contentContextFactory,
    IDbContextFactory<IdentityDbContext> identityContextFactory,
    IDbContextFactory<MediaDbContext> mediaContextFactory)
    : IQueryHandler<GetReactionsByPostQuery, PagedFeed<ReactionResponse>>
{
    public async Task<Result<PagedFeed<ReactionResponse>>> Handle(GetReactionsByPostQuery request, CancellationToken cancellationToken)
    {
        await using var contentContext = await contentContextFactory.CreateDbContextAsync(cancellationToken);
        await using var identityContext = await identityContextFactory.CreateDbContextAsync(cancellationToken);
        await using var mediaContext = await mediaContextFactory.CreateDbContextAsync(cancellationToken);

        var pagedReactions = await contentContext.Reactions
            .AsNoTracking()
            .Where(r => r.PostId == request.PostId && (!request.Type.HasValue || r.Type == request.Type.Value))
            .ToPagedFeed(request.Filter);

        var profiles = await identityContext.GetProfileLookupAsync(mediaContext, pagedReactions.Values.Select(r => r.UserId), cancellationToken);

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
