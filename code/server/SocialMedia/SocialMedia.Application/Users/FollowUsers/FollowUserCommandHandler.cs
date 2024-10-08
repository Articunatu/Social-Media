using SocialMedia.Application.Abstractions;
using SocialMedia.Domain.Abstractions;
using SocialMedia.Domain.Shared;
using SocialMedia.Domain.Users;

namespace SocialMedia.Application.Users.FollowUsers
{
    internal sealed class FollowUserCommandHandler(IUserRelationalRepository userRelational, IUserNoSqlRepository userNoSql, IUnitOfWork unitOfWork) : ICommandHandler<FollowUserCommand>
    {
        readonly IUserRelationalRepository _userRelational = userRelational;
        readonly IUserNoSqlRepository _userNoSql = userNoSql;
        readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(FollowUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Guid followerId = request.FollowerId;
                Guid followedId = request.FollowingId;
                bool isUnfollow = request.IsUnfollow;
                await NoSqlFollow(followerId, followedId, isUnfollow);
                await RelationalFollow(followerId, followedId, isUnfollow);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Failure(new Error(e.Message));
            }
        }

        private async Task RelationalFollow(Guid followerId, Guid followedId, bool isUnfollow)
        {
            var followerUser = await _userRelational.GetSingle(u => u.Id.Equals(followerId));
            var followedUser = await _userRelational.GetSingle(u => u.Id.Equals(followedId));
            var follow = User.Follow(followerId, followedId, followerUser, followedUser);
            _userRelational.Follow(follow, isUnfollow);
            if (isUnfollow)
            {
                followerUser.Following.Remove(followedUser);
                followedUser.Followers.Remove(followerUser);
            }
            else
            {
                followerUser.Following.Add(followedUser);
                followedUser.Followers.Add(followerUser);
            }
        }

        private async Task NoSqlFollow(Guid followerId, Guid followedId, bool isUnfollow)
        {
            var users = await _userNoSql.GetMultiple(u => u.Id == followerId || u.Id == followedId);
            var followerUser = users.FirstOrDefault(u => u.Id == followerId);
            var followedUser = users.FirstOrDefault(u => u.Id == followedId);
            if (isUnfollow)
            {
                followerUser.Following.Remove(followedId);
                followedUser.Following.Remove(followerId);
                await _userNoSql.UpdateMultiple([followerUser, followedUser]);
            }
            else
            {
                followerUser.Following.Add(followedId);
                followedUser.Followers.Add(followerId);
                await _userNoSql.UpdateMultiple([followerUser, followedUser]);
            }
        }
    }
}
