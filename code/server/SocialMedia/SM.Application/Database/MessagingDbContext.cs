using MediatR;
using Microsoft.EntityFrameworkCore;
using SM.Domain.Messaging;

namespace SM.Application.Database;

public sealed class MessagingDbContext(DbContextOptions<MessagingDbContext> options, IMediator mediator)
    : SocialMediaDbContextBase(options, mediator)
{
    public DbSet<Conversation> Conversations { get; set; } = default!;
    public DbSet<DirectMessage> DirectMessages { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Conversation>(entity =>
        {
            entity.ToTable(nameof(Conversations));
            entity.HasKey(conversation => conversation.Id);
        });

        builder.Entity<ConversationParticipant>(entity =>
        {
            entity.ToTable("ConversationParticipants");
            entity.HasKey(participant => new { participant.ConversationId, participant.UserId });
            entity.HasOne(participant => participant.Conversation)
                .WithMany(conversation => conversation.Participants)
                .HasForeignKey(participant => participant.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(participant => participant.UserId);
        });

        builder.Entity<DirectMessage>(entity =>
        {
            entity.ToTable(nameof(DirectMessages));
            entity.HasKey(message => message.Id);
            entity.Property(message => message.Content).IsRequired().HasMaxLength(2000);
            entity.HasIndex(message => new { message.ConversationId, message.TimeStamp });
            entity.HasOne(message => message.Conversation)
                .WithMany(conversation => conversation.Messages)
                .HasForeignKey(message => message.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasQueryFilter(message => !message.IsDeleted);
        });
    }
}