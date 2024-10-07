
namespace SocialMedia.Domain.Users
{
    public interface IUserNoSqlRepository : IRepository<UserNoSql, Guid>
    {
        Task Follow(Guid followerId, Guid followingId, UserDTO item);
    }

    public interface IUserRelationalRepository : IRepository<UserRelational, Guid>
    {
        Task Follow(FollowUser follow, bool isUnfollowg);
    }
}
