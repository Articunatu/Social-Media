using SocialMedia.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions;

namespace SocialMedia.Infrastructure.Repositories
{
    internal abstract class RelationalRepository<TEntity, TEntityId>(
        ApplicationDbContext dbContext)
        where TEntity : Entity<TEntityId>
    {
        public ApplicationDbContext table = dbContext;

        public async Task<TEntity?> GetSingle(TEntityId id)
        {
            return await table.Set<TEntity>().FirstOrDefaultAsync(e => e.Equals(id));
        }

        public async Task<IEnumerable<TEntity?>> GetMultiple(TEntityId id, string query)
        {
            return await table.Set<TEntity>().FromSqlRaw(query, id).ToArrayAsync();
        }

        public async Task Add(TEntity entity)
        {
            await table.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddMultiple(List<TEntity> entities)
        {
            await table.BulkInsertAsync(entities);
        }

        public async Task Delete(TEntityId id)
        {
            var entity = await GetSingle(id);
            if (entity is not null) 
                table.Set<TEntity>().Remove(entity);
        }

        public void Update(TEntity entity)
        {
            table.Set<TEntity>().Update(entity);
        }
        
        public async Task UpdateMultiple(List<TEntity> entities)
        {
            await table.BulkUpdateAsync(entities);
        }
    }
}
