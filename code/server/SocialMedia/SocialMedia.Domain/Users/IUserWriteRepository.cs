
namespace SocialMedia.Domain.Users
{
    public interface IUserWriteRepository
    {
        Task Add(UserRelational userRelational, UserNoSql userNoSql);
        Task Delete(Guid id);
        Task Update(UserRelational userRelational, UserNoSql userNoSql);
        Task Follow(Guid followerId, Guid followingdId, UserDTO item);
    }
}
