using Microsoft.EntityFrameworkCore;
using SocialMedia.Domain.Users;

namespace SocialMedia.Infrastructure.Repositories
{
    internal sealed class UserRelationalRepository(ApplicationDbContext context):
        RelationalRepository<UserRelational, Guid>(context), IUserRelationalRepository
    {
        public async Task Follow(Guid followerId, Guid followingdId, FollowUser follow)
        {
            var followerRelational = await table.Users.FirstOrDefaultAsync(u => u.Id == followerId);
            await table.Users.FirstOrDefaultAsync(following => following.Id == followerId).Result.Following.Add(followerRelational);
            await table.Follows.AddAsync(follow);
        }
    }
}
