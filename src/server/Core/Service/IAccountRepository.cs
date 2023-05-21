using Models.Models;
using Models.SubModels;

namespace Core.Service
{
    public interface IAccountRepository
    {
        Task<IEnumerable<AccountDto>> GetTop20PagedFollowers(Guid accountId);
        Task<IEnumerable<AccountDto>> GetTop20PagedFollowing(Guid accountId);
        Task<Account> GetAccount(Guid accountId);
        Task Create(Account created);
        Task Update(Account updated);
        Task Delete(Guid accountId);

        Task<IEnumerable<AccountDto>> GetTop20PagedMutuals(Guid accountId);
        Task<int> GetMutualsCount(Guid accountId);
    }
}
