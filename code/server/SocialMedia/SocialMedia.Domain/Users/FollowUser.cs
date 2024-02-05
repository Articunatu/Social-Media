
namespace SocialMedia.Domain.Users
{
    public sealed class FollowUser
    {
        public Guid Id { get; set; }
        public Guid FollowerId { get; set; }
        public Guid FollowingId { get; set; }

        public User Follower { get; set; }
        public User Following { get; set; }
    }
}
