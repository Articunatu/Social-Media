using MediatR;
using Microsoft.EntityFrameworkCore;
using SM.Domain.Photos;

namespace SM.Application.Database;

public class MediaDbContext(DbContextOptions<MediaDbContext> options, IMediator mediator)
    : SocialMediaDbContextBase(options, mediator)
{
    public DbSet<Photo> Photos { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Photo>(entity =>
        {
            entity.ToTable("Photos");
            entity.HasKey(photo => photo.Id);
            entity.Ignore(photo => photo.User);
            entity.Property(photo => photo.FileName)
                  .IsRequired()
                  .HasMaxLength(255);
            entity.Property(photo => photo.ContentType)
                  .IsRequired()
                  .HasMaxLength(100);
            entity.Property(photo => photo.CreatedAt)
                  .IsRequired();
        });
    }
}