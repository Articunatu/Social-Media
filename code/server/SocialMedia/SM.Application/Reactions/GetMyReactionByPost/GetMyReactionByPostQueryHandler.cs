using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Domain.Shared;
using System.Net;

namespace SM.Application.Reactions.GetMyReactionByPost;

internal class GetMyReactionByPostQueryHandler(IDbContextFactory<ApplicationDbContext> contextFactory)
    : IQueryHandler<GetMyReactionByPostQuery, ReactionResponse>
{
    public async Task<Result<ReactionResponse>> Handle(GetMyReactionByPostQuery request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var reaction = await context.Reactions
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.UserId == request.UserId && r.PostId == request.PostId, cancellationToken);

        if (reaction is null)
            return Result.Failure<ReactionResponse>(new Error("Reaction.NotFound"), HttpStatusCode.NotFound);

        var profile = await context.GetProfileAsync(reaction.UserId, cancellationToken);

        return Result.Success(reaction.MapToResponse(profile));
    }
}
