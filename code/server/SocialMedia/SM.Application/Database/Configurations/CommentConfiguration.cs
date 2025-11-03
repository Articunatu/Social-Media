using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SM.Domain.Messages;
using SM.Domain.Messages.ValueObjects;

namespace SM.Application.Database.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.Property(c => c.Content)
               .IsRequired()
               .HasMaxLength(Content.MaxLength);

        builder.HasOne(c => c.ParentPost)
               .WithMany(p => p.Comments)
               .HasForeignKey(c => c.ParentPostId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.ParentComment)
               .WithMany(c => c.Replies)
               .HasForeignKey(c => c.ParentCommentId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}