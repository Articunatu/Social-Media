using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SM.Domain.Content;
using SM.Domain.Content.ValueObjects;

namespace SM.Application.Database.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.Property(p => p.Content)
            .IsRequired()
            .HasMaxLength(ContentText.MaxLength);

        builder.HasOne(p => p.Author)
               .WithMany()
               .HasForeignKey(p => p.AuthorId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
