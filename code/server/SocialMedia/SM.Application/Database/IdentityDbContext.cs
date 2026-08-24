using MediatR;
using Microsoft.EntityFrameworkCore;
using SM.Application.Database.Configurations;
using SM.Domain.Authentication;
using SM.Domain.Content;
using SM.Domain.Users;

namespace SM.Application.Database;

public class IdentityDbContext(DbContextOptions<IdentityDbContext> options, IMediator mediator)
    : SocialMediaDbContextBase(options, mediator)
{
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Token> Tokens { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        NameTablesByEntities(builder);
        ApplySoftDeleteFilter(builder);

        builder.ApplyConfiguration(new UserConfiguration());
    }

    private static void NameTablesByEntities(ModelBuilder builder)
    {
        builder.Ignore<AuthoredContent>();
    }

    private static void ApplySoftDeleteFilter(ModelBuilder builder)
    {
        builder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
    }
}
