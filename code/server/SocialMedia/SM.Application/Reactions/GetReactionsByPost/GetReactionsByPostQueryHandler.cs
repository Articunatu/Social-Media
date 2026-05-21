using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Application.Shared.Models;
using SM.Domain.Shared;
using SM.Domain.Users.Extensions;

namespace SM.Application.Reactions.GetReactionsByPost;

internal class GetReactionsByPostQueryHandler(IDbContextFactory<ApplicationDbContext> contextFactory) 
    : IQueryHandler<GetReactionsByPostQuery, PagedFeed<ReactionResponse>>
{
    public async Task<Result<PagedFeed<ReactionResponse>>> Handle(GetReactionsByPostQuery request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var reactionsQuery = context.Reactions
            .Where(r => r.PostId == request.PostId && (!request.Type.HasValue || r.Type == request.Type.Value))
            .Include(r => r.User)
            .Select(r => new ReactionResponse
            (
                r.Id,
                r.Type,
                new ProfileInfo(r.UserId, r.User.Tag, r.User.GetFullName(), r.User.GetProfilePhoto())
            )).AsQueryable();

        var pagedReactions = await reactionsQuery.ToPagedFeed(request.Filter);

        return Result.Success(pagedReactions);
    }
}
