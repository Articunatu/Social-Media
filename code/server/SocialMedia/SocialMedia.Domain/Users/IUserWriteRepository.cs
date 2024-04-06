
namespace SocialMedia.Domain.Users
{
    public interface IUserWriteRepository
    {
        Task Add(User user);
        Task Delete(Guid id);
        Task Follow(Guid followerId, Guid followingdId);
    }
}
