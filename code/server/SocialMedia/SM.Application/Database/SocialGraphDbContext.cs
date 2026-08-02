using MediatR;
using Microsoft.EntityFrameworkCore;
using SM.Domain.SocialGraph;

namespace SM.Application.Database;

public class SocialGraphDbContext(DbContextOptions<SocialGraphDbContext> options, IMediator mediator)
    : SocialMediaDbContextBase(options, mediator)
{
    public DbSet<Follow> Follows { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Follow>(entity =>
        {
            entity.ToTable("Follows");
            entity.HasKey(f => new { f.FollowerId, f.FollowingId });
            entity.Property(f => f.FollowerId).HasColumnName("FollowersId");
            entity.Property(f => f.FollowingId).HasColumnName("FollowingId");
            entity.HasIndex(f => f.FollowingId);
        });
    }
}
