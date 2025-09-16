using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SM.Domain.Messages;

namespace SM.Application.Database.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.Property(p => p.Content)
            .IsRequired()
            .HasMaxLength(280);

        builder.HasOne(p => p.Author)
               .WithMany(u => u.AuthoredPosts)
               .HasForeignKey(p => p.AuthorId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

