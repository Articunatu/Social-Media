
using Models.DataTransferObjects;
using Models.Models;
using Models.SubModels;
using Models.SubModels.Account;
using Models.SubModels.Message;

namespace Core.Service
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> ReadAccountsFollowingById(Guid id);
        Task<IEnumerable<Account>> ReadAccountsFollowersById(Guid id);
        Task<Account> ReadSingle(Guid id);
        Task Create(Account created);
        Task AddPostToAccount(Account account, Post post);
        Task AddReactedPostToAccount(ReactedPost reactedPost, Account account);
        Task AddCommentToAccount(Account account, Models.SubModels.Account.A_Comment comment);
        Task UploadPhoto(Photo photo, Account account);
        Task<Account> GetAccountByTag(string tag);
        Task<Account> GetAccountByToken(string token);
    }
    
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> ReadAll();
        Task<IEnumerable<Message>> ReadTop5LatestMessagesFromAccountById(Guid id);
        Task<Message> ReadSingle(Guid id);
        Task Create(Message created);
        Task Update(Message updated);
        Task AddReactionToMessage(AccountDto reactor, Message message, Reaction reaction);
        Task AddReactionToComment(AccountDto reactor, Message message, Reaction reaction);
        Task AddCommentToPost(Post post, Models.SubModels.Message.M_Comment comment);
        Task Delete(Guid id);
    }
    
    public interface IReactionRepository
    {
        Task<IEnumerable<Reaction>> ReadAll();
        Task<Reaction> ReadSingle(int id);
        Task Create(Reaction created);
        Task Update(Reaction updated);
        Task Delete(int id);
    }
}
