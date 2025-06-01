using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SM.Domain.Users;

namespace SM.Application.Database.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Tag)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(25);
        
        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(40);
        
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasMany(u => u.AuthoredMessages)
               .WithOne(p => p.Author)
               .HasForeignKey(p => p.AuthorId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.Photos)
               .WithOne(p => p.User)
               .HasForeignKey(p => p.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(u => u.ProfilePhoto)
               .WithMany()
               .HasForeignKey(u => u.ProfilePhotoId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

