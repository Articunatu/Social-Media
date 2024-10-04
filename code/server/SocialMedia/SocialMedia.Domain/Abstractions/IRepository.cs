using SocialMedia.Domain.Users;

public interface IRepository<TEntity, TEntityId>
{
    Task<TEntity?> GetSingle(TEntityId id);
    Task<IEnumerable<TEntity?>> GetMultiple(TEntityId id, string query);
    Task Add(TEntity entity);
    Task AddMultiple(List<TEntity> entitis);
    Task Delete(TEntityId id);
    Task Update(TEntity entity);
    Task UpdateMultiple(List<TEntity> entitis);
}