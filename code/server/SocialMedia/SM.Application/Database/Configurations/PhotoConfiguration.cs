using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SM.Domain.Photos;
using SM.Domain.Users;

namespace SM.Application.Database.Configurations;

public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder.Property(p => p.FileName)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(p => p.ContentType)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(p => p.CreatedAt)
               .IsRequired();

        builder.HasOne(p => p.User)
               .WithMany(u => u.Photos)
               .HasForeignKey(p => p.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
