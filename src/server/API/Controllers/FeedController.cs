using API.ViewModels;
using AutoMapper;
using Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Formatting;
using System.Web.Http.Description;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedController : ControllerBase
    {
        readonly IMessageRepository _messageRepository;
        readonly IAccountRepository _accountRepository;
        readonly AuthenticationController _authenticationController;
        readonly IMapper _mapper;

        public FeedController(IAccountRepository accountRepository, IMessageRepository messageRepository, IMapper mapper, AuthenticationController authenticationController)
        {
            _accountRepository = accountRepository;
            _messageRepository = messageRepository;
            _mapper = mapper;
            _authenticationController = authenticationController;
        }

        [HttpGet]
        [ResponseType(typeof(FeedModel))]
        public async Task<IActionResult> GetFeedByLoginId()
        {
            var id = Guid.NewGuid();
            var logedInAccount = await _accountRepository.ReadSingle(id);
            var following = await _accountRepository.ReadAccountsFollowingById(id);
            var latestMessageByEachFollower = following.Select(async f => await _messageRepository.ReadTop5LatestMessagesFromAccountById(f.Id));
            var mappedPosts = latestMessageByEachFollower.Select(m => _mapper.Map<PostModel>(m));
            var sortedPosts = mappedPosts.OrderByDescending(p => p.Date).Take(25);
            FeedModel feed = new() { Posts = sortedPosts };
            return Ok(feed);
        }
    }
}
