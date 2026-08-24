using Microsoft.EntityFrameworkCore;
using SM.Application.Database;
using SM.Application.Photos;
using SM.Application.Shared.Models;
using SM.Domain.Photos;

namespace SM.Application.Shared.Extensions;

public static class ProfileQueryExtensions
{
    public static async Task<ProfileInfo> GetProfileAsync(this IdentityDbContext context, MediaDbContext mediaContext, Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        var profilePhoto = await mediaContext.Photos
            .AsNoTracking()
            .Where(p => p.UserId == userId && p.Type == PhotoType.Profile)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => p.MapToResponse())
            .FirstOrDefaultAsync(cancellationToken);

        return user?.MapToProfile(profilePhoto) ?? new ProfileInfo(userId, string.Empty, string.Empty, null);
    }

    public static async Task<Dictionary<Guid, ProfileInfo>> GetProfileLookupAsync(this IdentityDbContext context, MediaDbContext mediaContext, IEnumerable<Guid> userIds, CancellationToken cancellationToken)
    {
        var ids = userIds.Distinct().ToArray();

        var users = await context.Users
            .AsNoTracking()
            .Where(u => ids.Contains(u.Id))
            .ToListAsync(cancellationToken);

        var profilePhotos = await mediaContext.Photos
            .AsNoTracking()
            .Where(p => ids.Contains(p.UserId) && p.Type == PhotoType.Profile)
            .GroupBy(p => p.UserId)
            .Select(group => group.OrderByDescending(p => p.CreatedAt).First())
            .ToDictionaryAsync(p => p.UserId, p => p.MapToResponse(), cancellationToken);

        return users.ToDictionary(u => u.Id, u => u.MapToProfile(profilePhotos.GetValueOrDefault(u.Id)));
    }
}
