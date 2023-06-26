//using API.ViewModels;
//using AutoMapper;
//using Core.Service;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Azure.Cosmos;
//using Models.DataTransferObjects;
//using Models.Models;
//using Models.SubModels.Account;
//using Models.SubModels.Message;
//using System.Web.Http.Description;

////For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class PostController : ControllerBase
//    {
//        readonly IMessageRepository _messageRepository;
//        readonly IAccountRepository _accountRepository;
//        readonly IMapper _mapper;
//        readonly AuthenticationController _authenticationController;

//        public PostController(IMessageRepository messageRepository, IAccountRepository accountRepository, IMapper mapper, AuthenticationController authenticationController)
//        {
//            _messageRepository = messageRepository;
//            _accountRepository = accountRepository;
//            _mapper = mapper;
//            _authenticationController = authenticationController;
//        }

//        [HttpPost]
//        [ResponseType(typeof(void))]
//        public async Task<IActionResult> CreatePost([FromBody] Message message)
//        {
//            var accountId = await _authenticationController.GetLoggedInAccountId();

//            var query = new QueryDefinition(
//                query: "SELECT a.Following FROM Account a WHERE a.id @partitionKey")
//                .WithParameter("@partitionKey", accountId);

//            var account = await _accountRepository.GetSingleAccount(query);

//            message.Account = _mapper.Map<AccountDto>(account);

//            await _messageRepository.Create(message);

//            var post = _mapper.Map<Post>(message);
//            await _accountRepository.AddPostToAccount(accountId, post);

//            return Ok();
//        }

//        [HttpPost, Route("{messageId:Guid}")]
//        [ResponseType(typeof(void))]
//        public async Task<IActionResult> CreateComment([FromRoute] Guid messageId, [FromBody] M_Comment comment)
//        {
//            var accountId = await _authenticationController.GetLoggedInAccountId();

//            var message = await _messageRepository.ReadSingle(messageId);

//            var post = _mapper.Map<Post>(message);

//            await _messageRepository.AddCommentToPost(post, comment);

//            var accountsComment = new A_Comment(comment.Id, post, comment.Text, comment.Date, comment.Reactions);
//            await _accountRepository.AddCommentToAccount(accountId, accountsComment);

//            return Ok();
//        }

//        [HttpGet, Route("{messageId:Guid}")]
//        [ResponseType(typeof(CommentsPagedModel))]
//        public async Task<IActionResult> GetPostsComments([FromRoute] Guid messageId)
//        {
//            var query = new QueryDefinition(
//                query: "SELECT TOP 10 Comments FROM Message m where m.id @partitionKey")
//                .WithParameter("@partitionKey", messageId);

//            CommentsPagedModel comments = new()
//            {
//                Comments = await _messageRepository.GetAll<M_Comment>(query)
//            };

//            return Ok(comments);
//        }

//        [HttpGet, Route("{messageId:Guid}")]
//        //[ResponseType(typeof(void))]
//        public async Task<IActionResult> GetPostsReactions([FromRoute] Guid messageId)
//        {
//            var message = await _messageRepository.ReadSingle(messageId);

            

//            return Ok();
//        }
//    }
//}