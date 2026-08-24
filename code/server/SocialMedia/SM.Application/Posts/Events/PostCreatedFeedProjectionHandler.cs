using MediatR;
using Microsoft.EntityFrameworkCore;
using SM.Application.Database;
using SM.Domain.Content.Events;
using SM.Domain.Feed;

namespace SM.Application.Posts.Events;

internal sealed class PostCreatedFeedProjectionHandler(
    IDbContextFactory<SocialGraphDbContext> socialGraphContextFactory,
    IDbContextFactory<FeedDbContext> feedContextFactory)
    : INotificationHandler<PostCreatedDomainEvent>
{
    public async Task Handle(PostCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await using var socialGraphContext = await socialGraphContextFactory.CreateDbContextAsync(cancellationToken);
        await using var feedContext = await feedContextFactory.CreateDbContextAsync(cancellationToken);

        var recipients = await socialGraphContext.Follows
            .AsNoTracking()
            .Where(follow => follow.FollowingId == notification.AuthorId)
            .Select(follow => follow.FollowerId)
            .ToArrayAsync(cancellationToken);

        if (recipients.Length == 0)
            return;

        feedContext.FeedItems.AddRange(recipients.Select(recipientId =>
            FeedItem.Create(recipientId, notification.PostId, notification.AuthorId, notification.CreatedAt)));

        await feedContext.SaveChangesAsync(cancellationToken);
    }
}