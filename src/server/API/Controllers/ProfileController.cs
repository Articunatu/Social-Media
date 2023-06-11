using API.ViewModels;
using AutoMapper;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Description;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        readonly IAccountRepository _accountRepository;
        readonly IMapper _mapper;

        public ProfileController(IAccountRepository accountRepository, IMapper mapper)
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
            ProfileModel profile = _mapper.Map<ProfileModel>(account);
            var posts = account.Posts?.Select(p => _mapper.Map<PostModel>(p));
            ProfilePageModel profilePage = new(profile, posts);
            return Ok(profilePage);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ResponseType(typeof(ProfilePageModel))]
        public async Task<IActionResult> GetProfileFollowers([FromRoute] Guid id)
        {
            var account = await _accountRepository.ReadSingle(id);
            ProfileModel profile = _mapper.Map<ProfileModel>(account);
            var posts = account.Posts?.Select(p => _mapper.Map<PostModel>(p));
            ProfilePageModel profilePage = new(profile, posts);
            return Ok(profilePage);
        }
        
        [HttpGet]
        [Route("{id:guid}")]
        [ResponseType(typeof(ProfilePageModel))]
        public async Task<IActionResult> GetProfileFollowings([FromRoute] Guid id)
        {
            var account = await _accountRepository.ReadSingle(id);
            ProfileModel profile = _mapper.Map<ProfileModel>(account);
            var posts = account.Posts?.Select(p => _mapper.Map<PostModel>(p));
            ProfilePageModel profilePage = new(profile, posts);
            return Ok(profilePage);
        }
        
        [HttpGet]
        [Route("{id:guid}")]
        [ResponseType(typeof(ProfilePageModel))]
        public async Task<IActionResult> GetProfilePhotos([FromRoute] Guid id)
        {
            var account = await _accountRepository.ReadSingle(id);
            ProfileModel profile = _mapper.Map<ProfileModel>(account);
            var posts = account.Posts?.Select(p => _mapper.Map<PostModel>(p));
            ProfilePageModel profilePage = new(profile, posts);
            return Ok(profilePage);
        }
    }
}