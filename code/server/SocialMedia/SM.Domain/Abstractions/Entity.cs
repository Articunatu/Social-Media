namespace SM.Domain.Abstractions;

public abstract class Entity<TEntityId>(TEntityId id)
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public TEntityId Id { get; init; } = id;

    public IReadOnlyList<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents.ToList();
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void RaiseDomainEvent(IDomainEvent ev)
    {
        _domainEvents.Add(ev);
    }
}