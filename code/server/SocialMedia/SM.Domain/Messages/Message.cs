using System.ComponentModel.DataAnnotations.Schema;
using SM.Domain.Abstractions;

namespace SM.Domain.Messages;

[NotMapped]
public abstract class Message(Guid id) : SoftDeletableEntity<Guid>(id)
{
    public string Content { get; set; } = string.Empty;
    public DateTimeOffset TimeStamp { get; set; }
    public Guid AuthorId { get; set; }
}