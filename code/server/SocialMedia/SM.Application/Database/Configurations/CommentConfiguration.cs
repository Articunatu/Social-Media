using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SM.Domain.Messages;

namespace SM.Application.Database.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasOne(c => c.ParentPost)
               .WithMany(p => p.Replies)
               .HasForeignKey(c => c.ParentPostId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
