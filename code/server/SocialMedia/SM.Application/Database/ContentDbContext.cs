using MediatR;
using Microsoft.EntityFrameworkCore;
using SM.Domain.Content;
using SM.Domain.Content.ValueObjects;

namespace SM.Application.Database;

public class ContentDbContext(DbContextOptions<ContentDbContext> options, IMediator mediator)
    : SocialMediaDbContextBase(options, mediator)
{
    public DbSet<Post> Posts { get; set; } = default!;
    public DbSet<Comment> Comments { get; set; } = default!;
    public DbSet<Reaction> Reactions { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        NameTablesByEntities(builder);
        ApplySoftDeleteFilter(builder);

        builder.Entity<Post>(entity =>
        {
            entity.Property(p => p.Content)
                  .IsRequired()
                  .HasMaxLength(ContentText.MaxLength);
        });

        builder.Entity<Comment>(entity =>
        {
            entity.Property(c => c.Content)
                  .IsRequired()
                  .HasMaxLength(ContentText.MaxLength);

            entity.HasOne(c => c.ParentPost)
                  .WithMany(p => p.Comments)
                  .HasForeignKey(c => c.ParentPostId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(c => c.ParentComment)
                  .WithMany(c => c.Replies)
                  .HasForeignKey(c => c.ParentCommentId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Reaction>(entity =>
        {
            entity.ToTable(t => t.HasCheckConstraint("CK_Reaction_Target", "(CASE WHEN [PostId] IS NOT NULL THEN 1 ELSE 0 END + CASE WHEN [CommentId] IS NOT NULL THEN 1 ELSE 0 END) = 1"));

            entity.HasOne(r => r.Post)
                  .WithMany(p => p.Reactions)
                  .HasForeignKey(r => r.PostId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.Comment)
                  .WithMany(c => c.Reactions)
                  .HasForeignKey(r => r.CommentId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void NameTablesByEntities(ModelBuilder builder)
    {
        builder.Ignore<AuthoredContent>();
        builder.Entity<Post>().ToTable(nameof(Posts));
        builder.Entity<Comment>().ToTable(nameof(Comments));
    }

    private static void ApplySoftDeleteFilter(ModelBuilder builder)
    {
        builder.Entity<Post>().HasQueryFilter(m => !m.IsDeleted);
        builder.Entity<Comment>().HasQueryFilter(m => !m.IsDeleted);
    }
}
