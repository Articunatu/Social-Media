using SocialMedia.Domain.Users;
using System.Linq.Expressions;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        // Get methods for relational and NoSQL
        Task<UserRelational?> GetUserRelationalAsync(Expression<Func<UserRelational, bool>>? filter = null);
        Task<UserNoSql?> GetUserNoSqlAsync(Expression<Func<UserNoSql, bool>>? filter = null);

        // Combined CRUD methods
        Task CreateUserAsync(UserRelational userRelational, UserNoSql userNoSql);
        Task UpdateUserAsync(UserRelational userRelational, UserNoSql userNoSql);
        Task DeleteUserAsync(Guid id);
        void Follow(FollowUser follow, bool isUnfollow);
    }

    internal sealed class UserRepository(IUserRelationalRepository relationalRepository, IUserNoSqlRepository noSqlRepository) : IUserRepository
    {
        private readonly IUserRelationalRepository _relationalRepository = relationalRepository;
        private readonly IUserNoSqlRepository _noSqlRepository = noSqlRepository;

        // Get methods
        public async Task<UserRelational?> GetUserRelationalAsync(Expression<Func<UserRelational, bool>>? filter = null)
        {
            return await _relationalRepository.GetSingle(filter);
        }

        public async Task<UserNoSql?> GetUserNoSqlAsync(Expression<Func<UserNoSql, bool>>? filter = null)
        {
            return await _noSqlRepository.GetSingle(filter);
        }

        // Combined Create method
        public async Task CreateUserAsync(UserRelational userRelational, UserNoSql userNoSql)
        {
            // Perform both adds in one method call
            await _relationalRepository.Add(userRelational);
            await _noSqlRepository.Add(userNoSql);
        }

        // Combined Update method
        public async Task UpdateUserAsync(UserRelational userRelational, UserNoSql userNoSql)
        {
            // Perform both updates in one method call
            await _relationalRepository.Update(userRelational);
            await _noSqlRepository.Update(userNoSql);
        }

        // Combined Delete method
        public async Task DeleteUserAsync(Guid id)
        {
            // Perform both deletes in one method call
            await _relationalRepository.Delete(id);
            await _noSqlRepository.Delete(id);
        }

        // Follow functionality
        public void Follow(FollowUser follow, bool isUnfollow)
        {
            _relationalRepository.Follow(follow, isUnfollow);
        }
    }
}
