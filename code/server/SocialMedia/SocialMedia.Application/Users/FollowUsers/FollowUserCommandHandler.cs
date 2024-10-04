using SocialMedia.Application.Abstractions;
using SocialMedia.Domain.Abstractions;
using SocialMedia.Domain.Shared;
using SocialMedia.Domain.Users;

namespace SocialMedia.Application.Users.FollowUsers
{
    internal sealed class FollowUserCommandHandler : ICommandHandler<FollowUserCommand>
    {
        readonly IUserRelationalRepository _writeRepo;
        readonly IUserRepository _readRepo;
        readonly IUnitOfWork _unitOfWork;

        public FollowUserCommandHandler(IUserRelationalRepository userWrite, IUserRepository userRead, IUnitOfWork unitOfWork)
        {
            _writeRepo = userWrite;
            _readRepo = userRead;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(FollowUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _readRepo.
                Guid followerId = request.FollowerId;
                Guid followingId = request.FollowingId;
                User following = await _readRepo.GetSingle(followingId, "SELECT ");
                User follower = await _readRepo.GetSingle(followerId, "SELECT ");
                if(request.IsUnfollow)
                {
                    var followRef = await _readRepo.GetSingle(followerId, "Select");
                    follower.Following.Remove(followRef);
                    following.Followers.Remove(followRef);
                }
                else
                {
                    var followRef = User.Follow(followerId, followingId);
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
                return Result.Failure(new Error(e.Message));
            }
        }
    }
}
