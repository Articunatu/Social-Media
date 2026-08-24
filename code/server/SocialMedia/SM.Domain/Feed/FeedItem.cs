namespace SM.Domain.Feed;

public sealed class FeedItem
{
    public Guid RecipientId { get; private set; }
    public Guid PostId { get; private set; }
    public Guid AuthorId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    private FeedItem() { }

    public static FeedItem Create(Guid recipientId, Guid postId, Guid authorId, DateTimeOffset createdAt)
    {
        return new FeedItem
        {
            RecipientId = recipientId,
            PostId = postId,
            AuthorId = authorId,
            CreatedAt = createdAt
        };
    }
}