using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Models.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReactionController : ControllerBase
    {
        readonly IReactionRepository _reactionRepository;
        readonly IMessageRepository _messageRepository;
        readonly IAccountRepository _accountRepository;

        public ReactionController(IReactionRepository reaction, IMessageRepository message, IAccountRepository account)
        {
            _reactionRepository = reaction;
            _messageRepository = message;
            _accountRepository = account;
        }

        [HttpPut]
        [Route("react-post")]
        public async Task<IActionResult> ReactToPost([FromBody] Reaction reaction)
        {
            await _messageRepository.AddReactionToMessage(reaction, );
            await _accountRepository.AddReactedPostToAccount();
            return Ok();
        }
        
        [HttpPut]
        [Route("react-comment")]
        public async Task<IActionResult> ReactToComment([FromBody] Reaction reaction)
        {
            await _messageRepository.AddReactionToMessage(reaction, );
            await _accountRepository.UpdateSingleAccountsComment();
            return Ok();
        }
    }
}
