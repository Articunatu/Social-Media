using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SM.Domain.Users;
using SM.Domain.Users.ValueObjects;

namespace SM.Application.Database.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(x => x.Id)
            .IsUnique();
        
        builder.HasIndex(x => x.Tag)
            .IsUnique();
        
        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(u => u.Tag)
            .IsRequired()
            .HasMaxLength(Tag.MaxLength);
        
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(FirstName.MaxLength);
        
        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(LastName.MaxLength);
        
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(Email.MaxLength);

        builder.HasMany(u => u.AuthoredPosts)
               .WithOne(p => p.Author)
               .HasForeignKey(p => p.AuthorId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.Photos)
               .WithOne(p => p.User)
               .HasForeignKey(p => p.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

