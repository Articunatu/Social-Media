
using SocialMedia.Domain.Abstractions;

namespace SocialMedia.Domain.Users
{
    public interface IUserReadRepository
    {
        Task<IEnumerable<User>> GetMultiple<User>(int key, string _query);
        Task<User> GetSingle<User>(Guid key, string _query);
    }
}
