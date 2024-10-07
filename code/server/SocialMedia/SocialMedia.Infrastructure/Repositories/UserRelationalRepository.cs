using Microsoft.EntityFrameworkCore;
using SocialMedia.Domain.Users;

namespace SocialMedia.Infrastructure.Repositories
{
    internal sealed class UserRelationalRepository(ApplicationDbContext context):
        RelationalRepository<UserRelational, Guid>(context), IUserRelationalRepository
    {
        public void Follow(FollowUser follow, bool isUnfollow)
        {
            if(isUnfollow)
            {
                _context.Follows.Remove(follow);
            }
            else
            {
                _context.Follows.Add(follow);
            }
        }
    }
}
