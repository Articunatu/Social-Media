using AutoMapper;
using Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.SubModels.Account;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        readonly IAccountRepository _accountRepository;
        readonly IMessageRepository _messageRepository;
        readonly IMapper _mapper;

        public MessageController(IAccountRepository accountRepository, IMessageRepository messageRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddPost([FromBody] Post createdPost)
        {
            var loggedInAccountId = Guid.NewGuid();
            await _messageRepository.Create(createdPost, loggedInAccountId);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] Post createdComment)
        {
            var loggedInAccountId = Guid.NewGuid();
            await _messageRepository.Create(createdComment, loggedInAccountId);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddPrivateMessage([FromBody] Post createdPrivateMessage)
        {
            var loggedInAccountId = Guid.NewGuid();
            await _messageRepository.Create(createdPrivateMessage, loggedInAccountId);
            return Ok();
        }
    }
}
