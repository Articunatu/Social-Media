using System.Linq.Expressions;

public interface IRepository<TEntity, TEntityId>
{
    Task<TEntity?> GetSingle(Expression<Func<TEntity, bool>>? filter = null,
             Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryModifier = null);
    Task<IEnumerable<TEntity?>> GetMultiple(Expression<Func<TEntity, bool>>? filter = null,
             Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryModifier = null);
    Task Add(TEntity entity);
    Task AddMultiple(List<TEntity> entitis);
    Task Delete(TEntityId id);
    Task Update(TEntity entity);
    Task UpdateMultiple(List<TEntity> entitis);
}