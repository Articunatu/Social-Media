using AutoMapper;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Models.DataTransferObjects;
using Models.Models;
using Models.SubModels.Account;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        readonly IMessageRepository _messageRepository;
        readonly IAccountRepository _accountRepository;
        readonly IMapper _mapper;

        public PostController(IMessageRepository messageRepository, IAccountRepository accountRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("post")]
        public async Task<IActionResult> CreatePost(Message message, Account account)
        {
            message.Account = _mapper.Map<AccountDto>(account); 

            await _messageRepository.Create(message);

            var post = _mapper.Map<Post>(message);
            await _accountRepository.AddPostToAccount(account, post);

            return Ok();
        }
        
        [HttpPost]
        [Route("post")]
        public async Task<IActionResult> CreateComment(Account account, Message post, Comment comment)
        {
            message.Account = _mapper.Map<AccountDto>(account);

            await _messageRepository.AddCommentToPost(post, comment);

            var post = _mapper.Map<Post>(message);
            await _accountRepository.AddCommentToAccount(account, comment);

            return Ok();
        }

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> GetPostsComments(Account account, Message post, Comment comment)
        {
            message.Account = _mapper.Map<AccountDto>(account);

            await _messageRepository.AddCommentToPost(post, comment);

            var post = _mapper.Map<Post>(message);
            await _accountRepository.AddCommentToAccount(account, comment);

            return Ok();
        }
        
        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> GetPostsReactions(Account account, Message post, Comment comment)
        {
            message.Account = _mapper.Map<AccountDto>(account);

            await _messageRepository.AddCommentToPost(post, comment);

            var post = _mapper.Map<Post>(message);
            await _accountRepository.AddCommentToAccount(account, comment);

            return Ok();
        }
    }
}