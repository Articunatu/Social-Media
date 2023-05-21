using API.ViewModels;
using AutoMapper;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System.Web.Http.Description;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        readonly IAccountRepository _accountRepository;
        readonly IMessageRepository _messageRepository;
        readonly IMapper _mapper;

        public ProfileController(IAccountRepository accountRepository, IMessageRepository messageRepository,IMapper mapper)
        {
            _accountRepository = accountRepository;
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ResponseType(typeof(ProfilePageModel))]
        public async Task<IActionResult> GetProfilePage([FromRoute] Guid accountId)
        {
            var account = await _accountRepository.GetAccount(accountId);
            ProfileModel profile = _mapper.Map<ProfileModel>(account);
            var messages = await _messageRepository.GetTop10NewestMessagesFromAccount(accountId, null);
            var posts = messages.Results.Select(p => _mapper.Map<PostModel>(p));

            var followerCount = account.Followers.Count();
            var followingCount = account.Follwing.Count();
            var mutualCount = await _accountRepository.GetMutualsCount(accountId);

            ProfilePageModel profilePage = new(profile, posts, followerCount, followerCount, mutualCount);
            return Ok(profilePage);
        }
    }
}
