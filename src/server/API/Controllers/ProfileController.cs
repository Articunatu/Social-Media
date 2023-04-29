using API.ViewModels;
using AutoMapper;
using Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System.Web.Http.Description;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        readonly IRepository<Account> _accountRepository;
        readonly IMapper _mapper;

        public ProfileController(IRepository<Account> accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ResponseType(typeof(ProfilePageModel))]
        public async Task<IActionResult> GetProfileInfo([FromRoute] Guid id)
        {
            var account = await _accountRepository.ReadSingle(id);
            ProfilePageModel profilePage = _mapper.Map<ProfilePageModel>(account);
            return Ok(profilePage);
        }
    }
}
