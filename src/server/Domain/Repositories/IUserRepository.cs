using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserById(Guid userId, CancellationToken cancellationToken);
        Task<User> GetUserByTag(string tag);
        Task<bool> IsEmailUnique(string email);
    }
}
