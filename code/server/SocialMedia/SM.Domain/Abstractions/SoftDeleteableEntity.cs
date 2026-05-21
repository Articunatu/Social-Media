namespace SM.Domain.Abstractions;

public abstract class SoftDeletableEntity<TEntityId>
    : Entity<TEntityId>
{
    public bool IsDeleted { get; private set; }
    public DateTime? TimeOfDelete { get; private set; }

    protected SoftDeletableEntity(TEntityId id)
    {
        Id = id;
    }

    public virtual void SoftDelete()
    {
        if (IsDeleted)
            return;

        IsDeleted = true;
        TimeOfDelete = DateTime.UtcNow;
    }
}