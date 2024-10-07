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
                var followerUser = await _userRelational.GetSingle(u => u.Id.Equals(followerId));
                var followedUser = await _userRelational.GetSingle(u => u.Id.Equals(followedId));
                var follow = User.Follow(followerId, followedId, followerUser, followedUser);
                _userRelational.Follow();
                if (request.IsUnfollow)
                {
                    var followRef = await _userNoSql.GetSingle(followerId, "Select");
                    followerUser.Following.Remove(followRef);
                    followedUser.Followers.Remove(followRef);
                }
                else
                {
                    var followRef = User.Follow(followerId, followingId);
                    follower.Following.Add(followRef);
                    following.Followers.Add(followRef);
                }
                await _userRelational.Update(follower);
                await _userRelational.Update(following);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Failure(new Error(e.Message));
            }
        }
    }
}
