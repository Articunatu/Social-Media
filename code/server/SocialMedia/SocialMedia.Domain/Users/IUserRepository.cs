
namespace SocialMedia.Domain.Users
{
    public interface IUserNoSqlRepository : IRepository<UserNoSql, Guid> { }

    public interface IUserRelationalRepository : IRepository<UserRelational, Guid>
    {
        void Follow(FollowUser follow, bool isUnfollow);
    }
}
