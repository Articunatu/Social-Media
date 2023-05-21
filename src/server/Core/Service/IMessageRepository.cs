using Core.Paging;
using Models.Models;
using Models.SubModels.Account;

namespace Core.Service
{
    public interface IMessageRepository
    {
        Task<PagedResult<Message>> GetTop10NewestMessagesFromAccount(Guid accountId, string continuationToken);
        Task<IEnumerable<Message>> ReadAll();
        Task<Message?> GetMessagebyId(Guid id);
        Task Create(Post created, Guid accountId);
        Task Update(Message updated);
        Task Delete(Guid id);

    }
}
