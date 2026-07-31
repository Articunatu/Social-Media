using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SM.Domain.SocialGraph;
using SM.Domain.Users;

namespace SM.Application.Database.Configurations;

public class FollowConfiguration : IEntityTypeConfiguration<Follow>
{
    public void Configure(EntityTypeBuilder<Follow> builder)
    {
        builder.ToTable("Follows");

        builder.HasKey(f => new { f.FollowerId, f.FollowingId });

        builder.Property(f => f.FollowerId).HasColumnName("FollowersId");
        builder.Property(f => f.FollowingId).HasColumnName("FollowingId");

        builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(f => f.FollowerId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(f => f.FollowingId)
               .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasIndex(f => f.FollowingId);
    }
}
