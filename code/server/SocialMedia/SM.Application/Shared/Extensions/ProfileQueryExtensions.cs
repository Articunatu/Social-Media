using Microsoft.EntityFrameworkCore;
using SM.Application.Database;
using SM.Application.Shared.Models;

namespace SM.Application.Shared.Extensions;

public static class ProfileQueryExtensions
{
    public static async Task<ProfileInfo> GetProfileAsync(this ApplicationDbContext context, Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .AsNoTracking()
            .Include(u => u.Photos)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        return user?.MapToProfile() ?? new ProfileInfo(userId, string.Empty, string.Empty, null);
    }

    public static async Task<Dictionary<Guid, ProfileInfo>> GetProfileLookupAsync(this ApplicationDbContext context, IEnumerable<Guid> userIds, CancellationToken cancellationToken)
    {
        var ids = userIds.Distinct().ToArray();

        var users = await context.Users
            .AsNoTracking()
            .Include(u => u.Photos)
            .Where(u => ids.Contains(u.Id))
            .ToListAsync(cancellationToken);

        return users.ToDictionary(u => u.Id, u => u.MapToProfile());
    }
}
