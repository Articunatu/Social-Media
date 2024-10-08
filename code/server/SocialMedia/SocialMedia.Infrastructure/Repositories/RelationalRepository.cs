using SocialMedia.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions;
using System.Linq.Expressions;

namespace SocialMedia.Infrastructure.Repositories
{
    internal abstract class RelationalRepository<TEntity, TEntityId>(
        ApplicationDbContext context)
        where TEntity : Entity<TEntityId>
    {
        public ApplicationDbContext _context = context;

        public async Task<TEntity?> GetSingle(Expression<Func<TEntity, bool>>? filter = null,
             Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryModifier = null)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            if (queryModifier != null)
                query = queryModifier(query);

            return await query.FirstOrDefaultAsync();
        }

        //GetSingleById


        public async Task<IEnumerable<TEntity>> GetMultiple(
             Expression<Func<TEntity, bool>>? filter = null,
             Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryModifier = null)
        {
            // Get the base queryable set of entities
            var query = _context.Set<TEntity>().AsQueryable();

            // Apply filtering if provided
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Allow caller to modify the query (e.g., selecting fields, paging, etc.)
            if (queryModifier != null)
            {
                query = queryModifier(query);
            }

            // Execute the query and return the results
            return await query.ToArrayAsync();
        }

        public async Task Add(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddMultiple(List<TEntity> entities)
        {
            await _context.BulkInsertAsync(entities);
        }

        public async Task Delete(TEntityId id)
        {
            var entity = await GetSingle(e => e.Id.Equals(id));
            if (entity is not null) 
                _context.Set<TEntity>().Remove(entity);
        }

        public async Task Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }
        
        public async Task UpdateMultiple(List<TEntity> entities)
        {
            await _context.BulkUpdateAsync(entities);
        }
    }
}