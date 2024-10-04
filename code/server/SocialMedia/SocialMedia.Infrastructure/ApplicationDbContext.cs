using SocialMedia.Domain.Abstractions;
using SocialMedia.Domain.Users;
using SocialMedia.Domain.Messages;
using SocialMedia.Domain.Reactions;
using Microsoft.EntityFrameworkCore;

namespace SocialMedia.Infrastructure
{
    public sealed class ApplicationDbContext 
        
        (
        DbContextOptions options
        ) : DbContext(options), IUnitOfWork
    {
        public DbSet<UserRelational> Users { get; set; }
        public DbSet<PostRelational> Posts { get; set; }
        public DbSet<ReactionRelational> Reactions { get; set; }
        public DbSet<FollowUser> Follows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            modelBuilder.Entity<UserRelational>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<PostRelational>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<ReactionRelational>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<ReactionType>()
                .HasKey(r => r.Value);

            modelBuilder.Entity<FollowUser>()
                .HasKey(f => new { f.FollowerId, f.FollowingId }); // Composite key

            modelBuilder.Entity<FollowUser>()
                .HasOne(f => f.Follower)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FollowUser>()
                .HasOne(f => f.Following)
                .WithMany(u => u.Following)
                .HasForeignKey(f => f.FollowingId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserRelational>()
        .       OwnsOne(u => u.Token);

            // Optional: if you want to specify additional configuration for Token properties
            modelBuilder.Entity<UserRelational>()
                .OwnsOne(u => u.Token)
                .Property(t => t.Text)
                .IsRequired(); // Example: make Text property required

            //// Optional: if you have a navigation property from Token to User
            //modelBuilder.Entity<Token>()
            //    .HasOne(t => t.User)
            //    .WithOne(u => u.Token)
            //    .HasForeignKey<User>(u => u.Id); // Foreign key in User table
        }
    }
}
