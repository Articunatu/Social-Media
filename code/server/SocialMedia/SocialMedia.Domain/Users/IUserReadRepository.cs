
namespace SocialMedia.Domain.Users
{
    public interface IUserReadRepository
    {
        Task<IEnumerable<User?>> GetMultiple(object key, string _query);
        Task<User?> GetSingle(object key, string _query);
    }
}
