using System.ComponentModel.DataAnnotations.Schema;
using SM.Domain.Abstractions;

namespace SM.Domain.Messages;

[NotMapped]
public abstract class Message : Entity<Guid>, ISoftDeletable
{
    // Base properties shared by Post and Comment
    public string Content { get; set; } = string.Empty;
    public DateTimeOffset TimeStamp { get; set; }
    public Guid AuthorId { get; set; }
    public virtual bool IsDeleted { get; set; }
    public virtual DateTime? TimeOfDelete { get; set; }
}