namespace SM.Domain.Abstractions;

public abstract class Entity<TEntityId>
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public TEntityId Id { get; protected init; } = default!;

    protected Entity() { }

    protected Entity(TEntityId id)
    {
        Id = id;
    }

    public IReadOnlyList<IDomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();

    public void ClearDomainEvents() => _domainEvents.Clear();

    protected void RaiseDomainEvent(IDomainEvent @event) => _domainEvents.Add(@event);
}
