using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Domain.Photos;
using SM.Domain.Shared;
using SM.Domain.Users;
using System.Net;

namespace SM.Application.Users.GetProfile;

internal class GetProfileQueryHandler(IDbContextFactory<ApplicationDbContext> contextFactory)
    : IQueryHandler<GetProfileQuery, ProfileDetails>
{
    public async Task<Result<ProfileDetails>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var profileDetails = await context.Users
            .Where(u => u.Id == request.Id)
            .Select(u => new ProfileDetails
            {
                Profile = u.MapToProfile(),
                FollowersCount = u.Followers.Count(),
                FollowingCount = u.Following.Count(),
                BackgroundPhoto = u.Photos != null ? u.Photos.FirstOrDefault(p => p.Type == PhotoType.Background) : null,
                AboutMe = u.AuthoredMessages != null ? u.AuthoredMessages.OrderBy(am => am.TimeStamp).Last().Content : string.Empty 
            })
            .FirstOrDefaultAsync(cancellationToken);

        return profileDetails is null ? 
            Result.Failure<ProfileDetails>(new Error(UserErrors.NotFound), HttpStatusCode.NotFound) : 
            Result.Success(profileDetails);
    }
}
