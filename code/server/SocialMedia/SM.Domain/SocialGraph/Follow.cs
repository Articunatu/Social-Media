namespace SM.Domain.SocialGraph;

public sealed class Follow
{
    public Guid FollowerId { get; private set; }
    public Guid FollowingId { get; private set; }

    private Follow() { }

    public static Follow Create(Guid followerId, Guid followingId)
    {
        return new Follow
        {
            FollowerId = followerId,
            FollowingId = followingId
        };
    }
}
