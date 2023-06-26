using API.ViewModels;
using Microsoft.Azure.Cosmos;
using Models.DataTransferObjects;
using Models.Models;
using Models.SubModels;
using Models.SubModels.Account;
using Models.SubModels.Message;

namespace Core.Service
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetTop10FollowersById(Guid id);
        Task<IEnumerable<Account>> GetTop10FollowedAccountsById(Guid id);
        Task<Account> GetAccountById(Guid id);
        Task<Account> GetAccountByTag(string tag);
        Task<Account> GetAccountByToken(string token);
        Task AddNewAccount(Account created);
        Task AddPostToAccount(Guid accountId, Post post);
        Task AddReactedPostToAccount(ReactedPost reactedPost, Guid accountId);
        Task AddCommentToAccount(Guid accountId, A_Comment comment);
        Task UploadPhoto(Photo photo, Guid accountId);
        Task<IEnumerable<Photo>> GetTop10ProfilePhotos(Guid id);
    }
    
    public interface IMessageRepository
    {
        Task<IEnumerable<T>> GetAll<T>(QueryDefinition query);
        Task<IEnumerable<Message>> ReadAll();
        Task<IEnumerable<Message>> ReadTop5LatestMessagesFromAccountById(Guid id);
        Task<Message> ReadSingle(Guid id);
        Task Create(Message created);
        Task Update(Message updated);
        Task AddReactionToMessage(AccountDto reactor, Message message, Reaction reaction);
        Task AddReactionToComment(AccountDto reactor, Message message, Reaction reaction);
        Task AddCommentToPost(Post post, M_Comment comment);
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

    public interface IAuthenticationRepository
    {
        Task<Guid> SignUp(LoginModel request);
        Task<string> LoginAsync(LoginModel request);
        Task<Guid> GetCurrentAccountId();
    }
}
