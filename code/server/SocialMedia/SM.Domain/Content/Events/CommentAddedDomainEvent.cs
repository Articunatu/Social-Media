using SM.Domain.Abstractions;

namespace SM.Domain.Content.Events;

public record CommentAddedDomainEvent(Guid CommentId, Guid PostId, Guid AuthorId, DateTimeOffset CreatedAt) : IDomainEvent;
