using Microsoft.EntityFrameworkCore;
using SM.Domain.Authentication;
using SM.Domain.Content;
using SM.Domain.Messages;
using SM.Domain.Photos;
using SM.Domain.Users;

namespace SM.Application.Database;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Post> Posts { get; set; } = default!;
    public DbSet<Comment> Comments { get; set; } = default!;
    public DbSet<Reaction> Reactions { get; set; } = default!;
    public DbSet<Token> Tokens { get; set; } = default!;
    public DbSet<Photo> Photos { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        NameTablesByEntities(builder);
        ApplySoftDeleteFilter(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    private static void NameTablesByEntities(ModelBuilder builder)
    {
        builder.Ignore<Message>();
        builder.Entity<Post>().ToTable(nameof(Posts));
        builder.Entity<Comment>().ToTable(nameof(Comments));
    }

    private static void ApplySoftDeleteFilter(ModelBuilder builder)
    {
        builder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
        builder.Entity<Post>().HasQueryFilter(m => !m.IsDeleted);
        builder.Entity<Comment>().HasQueryFilter(m => !m.IsDeleted);
    }
}
