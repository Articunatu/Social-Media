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

        var reactions = context.Reactions
            .Where(r => r.MessageId == request.PostId
                && r.Type == request.Type)
            .Include(r => r.User)
            .Select(r => new ReactionResponse
            (
                r.Id,
                r.Type,
                new ProfileInfo(r.UserId, r.User.Tag, r.User.FirstName + " " + r.User.LastName, r.User.GetProfilePhoto())
            )).AsQueryable();

        var pagedReactions = await reactions.ToPagedFeed(request.Filter);

        return Result.Success(pagedReactions);
    }
}
