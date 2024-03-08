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
            await _dbContext.SaveChangesAsync();
            await _container.CreateItemAsync(entity);
        }

        public async Task Delete(TEntityId id)
        {
            var entityEFCore = await _dbContext.Set<TEntity>().FindAsync(id);
            var entity = entityEFCore;
            if (entity != null)
            {
                _dbContext.Set<TEntity>().Remove(entity);
                await _dbContext.SaveChangesAsync();
                await _container.DeleteItemAsync<User>(id.ToString(), new PartitionKey(id.ToString()));
            }
        }

        public async void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            await _dbContext.SaveChangesAsync();
            await _container.UpsertItemAsync(entity);
        }
    }
}
