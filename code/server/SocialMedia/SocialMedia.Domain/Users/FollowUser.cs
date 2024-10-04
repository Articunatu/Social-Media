
namespace SocialMedia.Domain.Users
{
    public sealed class FollowUser(Guid id, Guid followerId, Guid followingId)
    {
        public Guid Id { get; set; } = id;
        public Guid FollowerId { get; set; } = followerId;
        public Guid FollowingId { get; set; } = followingId;
        public UserRelational Follower { get; set; }
        public UserRelational Following { get; set; }
    }
}
