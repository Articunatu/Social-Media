using SM.Domain.Abstractions;

namespace SM.Domain.Content.Events;

public record PostCreatedDomainEvent(Guid PostId, Guid AuthorId, DateTimeOffset CreatedAt) : IDomainEvent;
