using SocialMedia.Application.Abstractions;
using SocialMedia.Domain.Abstractions;
using SocialMedia.Domain.Shared;
using SocialMedia.Domain.Users;

namespace SocialMedia.Application.Users.FollowUsers
{
    internal sealed class FollowUserCommandHandler : ICommandHandler<FollowUserCommand, Guid>
    {
        readonly IUserWriteRepository _writeRepo;
        readonly IUserReadRepository _readRepo;
        readonly IUnitOfWork _unitOfWork;

        public FollowUserCommandHandler(IUserWriteRepository userWrite, IUserReadRepository userRead, IUnitOfWork unitOfWork)
        {
            _writeRepo = userWrite;
            _readRepo = userRead;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(FollowUserCommand request, CancellationToken cancellationToken)
        {
            Guid followerId = request.FollowerId;
            Guid followingId = request.FollowingId;
            try
            {
                var follower = await _readRepo.GetSingle<User>(followerId, "SELECT ");
                var following = await _readRepo.GetSingle<User>(followingId, "SELECT ");
                if(request.isUnfollow)
                {
                    var followRef = await _readRepo.GetSingle<FollowUser>(followerId, "Select");
                    follower.Following.Remove(followRef);
                    following.Followers.Remove(followRef);
                }
                else
                {
                    var followRef = User.Follow(follower, following, followerId, followingId);
                    follower.Following.Add(followRef);
                    following.Followers.Add(followRef);
                }
                await _writeRepo.Update(follower);
                await _writeRepo.Update(following);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Failure<Guid>(new Error(e.Message));
            }
        }
    }
}
