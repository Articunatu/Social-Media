using AutoMapper;
using Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DataTransferObjects;
using Models.Models;
using Models.SubModels;
using Models.SubModels.Account;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        readonly IMessageRepository _messageRepository;
        readonly IAccountRepository _accountRepository;
        readonly IMapper _mapper;

        public PhotoController(IMessageRepository message, IAccountRepository account, IMapper mapper)
        {
            _accountRepository = account;
            _messageRepository = message;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("post")]
        public async Task<IActionResult> UploadPhoto(Photo photo, Account account)
        {
            await _accountRepository.UploadPhoto(photo, account);
            return Ok();
        }
    }
}