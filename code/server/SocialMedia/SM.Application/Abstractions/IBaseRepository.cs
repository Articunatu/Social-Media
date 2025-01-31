
namespace SM.Application.Abstractions
{
    public interface IBaseRepository<TEntity, TEntityId>
    {
        TEntity GetOne();
        IEnumerable<TEntity> GetMany();
        Task Create(TEntity entity); 
        Task Delete(TEntityId id); 
        Task Update(TEntity entity, TEntityId id); 
    }
}
