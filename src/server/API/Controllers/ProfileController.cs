using API.ViewModels;
using AutoMapper;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Models.DataTransferObjects;
using Models.SubModels;
using System.Web.Http.Description;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        readonly IAccountRepository _accountRepository;
        readonly IMapper _mapper;

        public ProfileController(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ResponseType(typeof(ProfilePageModel))]
        public async Task<IActionResult> GetProfileInfo([FromRoute] Guid id)
        {
            var query = new QueryDefinition(
                query: "SELECT a.Tag, a.FirstName, a.LastName, a.ProfilePhoto FROM Account a" +
                "JOIN (SELECT TOP 10 AccountId, MAX(PostDate) AS LatestPostDate" +
                "FROM Post WHERE AccountId = @partitionKey" +
                "GROUP BY AccountId) p ON a.id = p.AccountId" +
                "ORDER BY p.LatestPostDate DESC")
            .WithParameter("@partitionKey", id);
            var account = await _accountRepository.GetAccountById(id);
            ProfileModel profile = _mapper.Map<ProfileModel>(account);
            var posts = account.Posts?.Select(p => _mapper.Map<PostModel>(p));
            ProfilePageModel profilePage = new(profile, posts);
            return Ok(profilePage);
        }

        [HttpGet]
        [Route("{id:guid}/followers")]
        [ResponseType(typeof(FollowerModel))]
        public async Task<IActionResult> GetProfileFollowers([FromRoute] Guid id)
        {
            FollowerModel followers = new()
            {
                Followers = (await _accountRepository.GetTop10FollowersById(id)).Select(account => _mapper.Map<AccountDto>(account))
            };
            return Ok(followers);
        }

        [HttpGet]
        [Route("{id:guid}/following")]
        [ResponseType(typeof(FollowingModel))]
        public async Task<IActionResult> GetProfileFollowings([FromRoute] Guid id)
        {
            FollowingModel followedAccounts = new()
            {
                Following = (await _accountRepository.GetTop10FollowedAccountsById(id)).Select(account => _mapper.Map<AccountDto>(account))
            };

            return Ok(followedAccounts);
        }
    }
}