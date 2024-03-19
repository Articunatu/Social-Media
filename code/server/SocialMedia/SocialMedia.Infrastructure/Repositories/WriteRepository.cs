using Microsoft.Azure.Cosmos;
using SocialMedia.Domain.Abstractions;

namespace SocialMedia.Infrastructure.Repositories
{

    internal abstract class WriteRepository<TEntity, TEntityId>(
        ApplicationDbContext dbContext, 
        Container container)
        where TEntity : Entity<TEntityId>
    {
        protected readonly ApplicationDbContext _dbContext = dbContext;
        protected readonly Container _container = container;


        public async Task Add(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _container.CreateItemAsync(entity);
        }

        public async Task Delete(TEntityId id)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(id);
            if (entity != null)
            {
                _dbContext.Set<TEntity>().Remove(entity);
                await DeleteCosmos(id, entity);
            }
        }

        private async Task DeleteCosmos(TEntityId id, TEntity? entity)
        {
            if (entity is ISoftDeletable softDeletableEntity)
            {
                softDeletableEntity.IsDeleted = true;
                softDeletableEntity.TimeOfDelete = DateTime.UtcNow;
                await _container.UpsertItemAsync(softDeletableEntity);
            }
            else
                await _container.DeleteItemAsync<TEntity>(id, new PartitionKey(id.ToString()));
        }

        public async void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            await _container.UpsertItemAsync(entity);
        }
    }
}
