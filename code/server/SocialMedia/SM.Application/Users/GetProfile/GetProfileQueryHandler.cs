using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Domain.Photos;
using SM.Domain.Shared;
using SM.Domain.Users;
using System.Net;

namespace SM.Application.Users.GetProfile;

internal class GetProfileQueryHandler(
    IDbContextFactory<IdentityDbContext> identityContextFactory,
    IDbContextFactory<SocialGraphDbContext> socialGraphContextFactory,
    IDbContextFactory<ContentDbContext> contentContextFactory)
    : IQueryHandler<GetProfileQuery, ProfileDetails>
{
    public async Task<Result<ProfileDetails>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        await using var identityContext = await identityContextFactory.CreateDbContextAsync(cancellationToken);
        await using var socialGraphContext = await socialGraphContextFactory.CreateDbContextAsync(cancellationToken);
        await using var contentContext = await contentContextFactory.CreateDbContextAsync(cancellationToken);

        var profileDetails = await identityContext.Users
            .Where(u => u.Id == request.Id)
            .Select(u => new ProfileDetails
            {
                Profile = u.MapToProfile(),
                FollowersCount = socialGraphContext.Follows.Count(f => f.FollowingId == u.Id),
                FollowingCount = socialGraphContext.Follows.Count(f => f.FollowerId == u.Id),
                IsFollowedByCurrentUser = request.ViewerId != Guid.Empty && socialGraphContext.Follows.Any(f => f.FollowingId == u.Id && f.FollowerId == request.ViewerId),
                BackgroundPhoto = u.Photos.Where(p => p.Type == PhotoType.Background)
                    .OrderByDescending(p => p.CreatedAt)
                    .FirstOrDefault(),
                AboutMe = string.Empty
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (profileDetails is null)
            return Result.Failure<ProfileDetails>(UserErrors.NotFound, HttpStatusCode.NotFound);

        profileDetails.AboutMe = await contentContext.Posts
            .Where(p => p.AuthorId == request.Id)
            .OrderByDescending(p => p.TimeStamp)
            .Select(p => p.Content)
            .FirstOrDefaultAsync(cancellationToken) ?? string.Empty;

        return Result.Success(profileDetails);
    }
}
