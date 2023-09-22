using API.ViewModels;
using AutoMapper;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Caching.Memory;
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
        readonly IMemoryCache _cache;
        readonly ILogger _logger;


        public ProfileController(IAccountRepository accountRepository, IMapper mapper, ILogger logger)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _logger = logger;
            //_cache = memoryCache;
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ResponseType(typeof(ProfilePageModel))]
        public async Task<IActionResult> GetProfileInfo([FromRoute] Guid id)
        {
            //var cacheKey = $"ProfileInfo_{id}";

            //if (_cache.TryGetValue(cacheKey, out ProfilePageModel profilePage))
            //{
            //    return Ok(profilePage);
            //}

            var account = await _accountRepository.GetAccountById(id);
            ProfileModel profile = _mapper.Map<ProfileModel>(account);
            var posts = account.Posts?.Select(p => _mapper.Map<PostModel>(p));
            var profilePage = new ProfilePageModel(profile, posts);

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // Adjust the expiration time as needed
            };

            //_cache.Set(cacheKey, profilePage, cacheEntryOptions);

            return Ok(profilePage);
        }


        [HttpGet]
        [Route("{id:guid}/followers")]
        [ResponseType(typeof(FollowerModel))]
        public async Task<IActionResult> GetProfileFollowers([FromRoute] Guid id)
        {
            FollowerModel followers = new()
            {
                Followers = (await _accountRepository.GetTop10FollowersById(id)).Results.Select(account => _mapper.Map<AccountDto>(account))
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
                Following = (await _accountRepository.GetTop10FollowedAccountsById(id)).Results.Select(account => _mapper.Map<AccountDto>(account))
            };

            return Ok(followedAccounts);
        }

        [HttpPost]
        [Route("add-fake-accounts")]
        public async Task<IActionResult> Add10FakeAccounts()
        {
            string dateText = DateTime.Now.ToString();
            _logger.LogInformation("{Count} new accounts were generated {Date}.", 10, dateText);
            var accounts = await _accountRepository.Generate10FakeAccounts();
            return Ok(accounts);
        }
    }
}