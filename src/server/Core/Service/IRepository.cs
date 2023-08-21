using API.ViewModels;
using Core.Paging;
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
        Task<PagedResult<Account>> GetTop10FollowersById(Guid id, string? continuationToken = null, int pageSize = 10);
        Task<PagedResult<Account>> GetTop10FollowedAccountsById(Guid id, string? continuationToken = null, int pageSize = 10);
        Task<Account> GetAccountById(Guid id);
        Task<Account> GetAccountByTag(string tag);
        Task<Account> GetAccountByToken(string token);
        Task AddNewAccount(Account created);
        Task AddPostToAccount(Guid accountId, Post post);
        Task AddReactedPostToAccount(ReactedPost reactedPost, Guid accountId);
        Task AddCommentToAccount(Guid accountId, A_Comment comment);
        Task UploadPhoto(Photo photo, Guid accountId);
        Task<PagedResult<Photo>> GetTop10ProfilePhotos(Guid id, string? continuationToken = null, int pageSize = 10);
    }

    public interface IMessageRepository
    {
        Task Create(Message created);
        Task Update(Message updated);
        Task AddReactionToMessage(AccountDto reactor, Message message, Reaction reaction);
        Task AddReactionToComment(AccountDto reactor, Message message, Reaction reaction);
        Task AddCommentToPost(Post post, M_Comment comment);
        Task Delete(Guid id);
        Task<PagedResult<Message>> Get10LatestPostsByAccountId(Guid accountId, string? continuationToken = null, int pageSize = 10);
        Task<Message> GetMessageById(Guid id);
        Task<IEnumerable<Message>> GetTrendingPosts();
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
