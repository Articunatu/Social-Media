using Microsoft.Azure.Cosmos;
using SocialMedia.Domain.Abstractions;
using SocialMedia.Domain.Messages;

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
                //_container.DeleteItemAsync<User>(id, new PartitionKey(id));
            }
        }

        public void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            _container.UpsertItemAsync(entity);
        }
    }
}
