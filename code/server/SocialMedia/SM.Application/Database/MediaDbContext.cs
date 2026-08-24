using MediatR;
using Microsoft.EntityFrameworkCore;
using SM.Application.Database.Configurations;
using SM.Domain.Photos;

namespace SM.Application.Database;

public class MediaDbContext(DbContextOptions<MediaDbContext> options, IMediator mediator)
    : SocialMediaDbContextBase(options, mediator)
{
    public DbSet<Photo> Photos { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
          builder.ApplyConfiguration(new PhotoConfiguration());
    }
}