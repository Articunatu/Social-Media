using MediatR;
using Microsoft.EntityFrameworkCore;
using SM.Domain.Feed;

namespace SM.Application.Database;

public class FeedDbContext(DbContextOptions<FeedDbContext> options, IMediator mediator)
    : SocialMediaDbContextBase(options, mediator)
{
    public DbSet<FeedItem> FeedItems { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<FeedItem>(entity =>
        {
            entity.ToTable("UserFeedItems");
            entity.HasKey(item => new { item.RecipientId, item.PostId });
            entity.HasIndex(item => new { item.RecipientId, item.CreatedAt });
        });
    }
}