using Microsoft.EntityFrameworkCore;
using SM.Domain.Authentication;
using SM.Domain.Messages;
using SM.Domain.Photos;
using SM.Domain.Reactions;
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

        // Ignore abstract base so only concrete tables exist
        builder.Ignore<Message>();

        builder.Entity<Post>().ToTable(nameof(Posts));
        builder.Entity<Comment>().ToTable(nameof(Comments));

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
