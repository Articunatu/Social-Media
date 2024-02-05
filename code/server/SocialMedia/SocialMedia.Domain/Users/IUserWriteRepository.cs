
namespace SocialMedia.Domain.Users
{
    public interface IUserWriteRepository
    {
        Task Add(User user);
        Task Delete(Guid id);
        void Update(User user);
    }
}
