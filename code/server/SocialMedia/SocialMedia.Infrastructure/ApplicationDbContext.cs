using Microsoft.EntityFrameworkCore;
using SocialMedia.Domain.Abstractions;
using SocialMedia.Domain.Messages;
using SocialMedia.Domain.Reactions;
using SocialMedia.Domain.Users;

namespace SocialMedia.Infrastructure
{
    public sealed class ApplicationDbContext(
        DbContextOptions options
        ) : DbContext(options), IUnitOfWork
    {

        //private static readonly JsonSerializerSettings JsonSerializerSettings = new()
        //{
        //    TypeNameHandling = TypeNameHandling.All
        //};

        //private readonly IPublisher _publisher = publisher;

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; init; }
        public DbSet<Reaction> Reactions { get; init; }
        public DbSet<FollowUser> Follows { get; init; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<Message>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Reaction>()
                .HasKey(r => r.Id);
            
            modelBuilder.Entity<ReactionType>()
                .HasKey(r => r.Value);
            
            modelBuilder.Entity<FollowUser>()
                .HasKey(f => f.Id);

            modelBuilder.Entity<FollowUser>()
                .HasOne(f => f.Follower)
                .WithMany(u => u.Following)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.Restrict); // Adjust the delete behavior as needed

            modelBuilder.Entity<FollowUser>()
                .HasOne(f => f.Following)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.FollowingId)
                .OnDelete(DeleteBehavior.Restrict); // Adjust the delete behavior as needed

            base.OnModelCreating(modelBuilder);
        }
    }
}
