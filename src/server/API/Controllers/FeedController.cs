//using API.ViewModels;
//using AutoMapper;
//using Core.Service;
//using Microsoft.AspNetCore.Mvc;
//using System.Web.Http.Description;

//namespace API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class FeedController : ControllerBase
//    {
//        readonly IMessageRepository _messageRepository;
//        readonly ProfileController _profile;
//        readonly AuthenticationController _authenticationController;
//        readonly IMapper _mapper;

//        public FeedController(ProfileController accountRepository, IMessageRepository messageRepository, IMapper mapper, AuthenticationController authenticationController)
//        {
//            _profile = accountRepository;
//            _messageRepository = messageRepository;
//            _mapper = mapper;
//            _authenticationController = authenticationController;
//        }

//        [HttpGet]
//        [ResponseType(typeof(FeedModel))]
//        public async Task<IActionResult> GetFeed()
//        {
//            var loggedInId = await _authenticationController.GetLoggedInAccountId();
//            var following = await _profile.GetProfileFollowings(loggedInId).;
//            var latestMessageByEachFollower = following.Select(async f => await _messageRepository.ReadTop5LatestMessagesFromAccountById(f.Id));
//            var mappedPosts = latestMessageByEachFollower.Select(m => _mapper.Map<PostModel>(m));
//            var sortedPosts = mappedPosts.OrderByDescending(p => p.Date).Take(25);
//            FeedModel feed = new() { Posts = sortedPosts };
//            return Ok(feed);
//        }
//    }
//}
