
namespace SocialMedia.Domain.Users
{
    public interface IUserReadRepository
    {
        Task<IEnumerable<User>> GetMultiple<User>(object key, string _query);
        Task<User> GetSingle<User>(object key, string _query);
    }
}
