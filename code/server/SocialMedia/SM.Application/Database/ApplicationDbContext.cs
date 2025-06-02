using Microsoft.EntityFrameworkCore;
using SM.Domain.Authentication;
using SM.Domain.Messages;
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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
