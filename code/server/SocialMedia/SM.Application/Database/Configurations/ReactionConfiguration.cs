using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SM.Domain.Content;

namespace SM.Application.Database.Configurations;

public class ReactionConfiguration : IEntityTypeConfiguration<Reaction>
{
    public void Configure(EntityTypeBuilder<Reaction> builder)
    {
        // Ensure only one target (Post or Comment) is referenced
        builder.ToTable(t => t.HasCheckConstraint("CK_Reaction_Target", "(CASE WHEN [PostId] IS NOT NULL THEN 1 ELSE 0 END + CASE WHEN [CommentId] IS NOT NULL THEN 1 ELSE 0 END) = 1"));

        // Restrict direct cascade from Post to Reactions to avoid multiple cascade paths (Post -> Comments -> Reactions and Post -> Reactions)
        builder.HasOne(r => r.Post)
               .WithMany(p => p.Reactions)
               .HasForeignKey(r => r.PostId)
               .OnDelete(DeleteBehavior.Restrict);

        // Keep cascade from Comment to Reactions so deleting a comment removes its reactions
        builder.HasOne(r => r.Comment)
               .WithMany(c => c.Reactions)
               .HasForeignKey(r => r.CommentId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.User)
               .WithMany()
               .HasForeignKey(r => r.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(r => new { r.PostId, r.UserId })
               .IsUnique()
               .HasFilter("[PostId] IS NOT NULL");

        builder.HasIndex(r => new { r.CommentId, r.UserId })
               .IsUnique()
               .HasFilter("[CommentId] IS NOT NULL");
    }
}
