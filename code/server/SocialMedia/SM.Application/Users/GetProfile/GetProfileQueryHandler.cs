using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Photos;
using SM.Application.Shared.Extensions;
using SM.Domain.Photos;
using SM.Domain.Shared;
using SM.Domain.Users;
using System.Net;

namespace SM.Application.Users.GetProfile;

internal class GetProfileQueryHandler(
    IDbContextFactory<IdentityDbContext> identityContextFactory,
    IDbContextFactory<SocialGraphDbContext> socialGraphContextFactory,
    IDbContextFactory<ContentDbContext> contentContextFactory,
    IDbContextFactory<MediaDbContext> mediaContextFactory)
    : IQueryHandler<GetProfileQuery, ProfileDetails>
{
    public async Task<Result<ProfileDetails>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        await using var identityContext = await identityContextFactory.CreateDbContextAsync(cancellationToken);
        await using var socialGraphContext = await socialGraphContextFactory.CreateDbContextAsync(cancellationToken);
        await using var contentContext = await contentContextFactory.CreateDbContextAsync(cancellationToken);
        await using var mediaContext = await mediaContextFactory.CreateDbContextAsync(cancellationToken);

        var user = await identityContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (user is null)
            return Result.Failure<ProfileDetails>(UserErrors.NotFound, HttpStatusCode.NotFound);

        var profilePhoto = await mediaContext.Photos
            .AsNoTracking()
            .Where(p => p.UserId == request.Id && p.Type == PhotoType.Profile)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => p.MapToResponse())
            .FirstOrDefaultAsync(cancellationToken);

        var backgroundPhoto = await mediaContext.Photos
            .AsNoTracking()
            .Where(p => p.UserId == request.Id && p.Type == PhotoType.Background)
            .OrderByDescending(p => p.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        var profileDetails = new ProfileDetails
        {
            Profile = user.MapToProfile(profilePhoto),
            FollowersCount = await socialGraphContext.Follows.CountAsync(f => f.FollowingId == request.Id, cancellationToken),
            FollowingCount = await socialGraphContext.Follows.CountAsync(f => f.FollowerId == request.Id, cancellationToken),
            IsFollowedByCurrentUser = request.ViewerId != Guid.Empty && await socialGraphContext.Follows.AnyAsync(
                f => f.FollowingId == request.Id && f.FollowerId == request.ViewerId,
                cancellationToken),
            BackgroundPhoto = backgroundPhoto,
        };

        profileDetails.AboutMe = await contentContext.Posts
            .Where(p => p.AuthorId == request.Id)
            .OrderByDescending(p => p.TimeStamp)
            .Select(p => p.Content)
            .FirstOrDefaultAsync(cancellationToken) ?? string.Empty;

        return Result.Success(profileDetails);
    }
}
