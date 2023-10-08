using Application.Users.Commands.AddUserCommand;
using Application.Users.Queries.GetUserById;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;
using Presentation.Models.ResponseModels;
using Presentation.Models.ViewModels;
using System.Web.Http.Description;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class ProfileController : ApiController
    {
        public ProfileController(ISender sender) : base(sender) { }

        [HttpPost]
        [Route("register")]
        [ResponseType(typeof(RegisterProfileResponseModel))]
        public async Task<IActionResult> RegisterProfile(CancellationToken cancellationToken)
        {
            var command = new AddUserCommand(
                "artciunatu",
                "artciunatu@gmail.com",
                "Articuno",
                "Natu");

            var response = await _sender.Send(command, cancellationToken);

            if (response.IsFailure)
            {
                return HandleFailure(response);
            }

            return response.IsSuccess ? Ok(response) : BadRequest(response.Error);
        }

        [HttpGet]
        [Route("{profileId}")]
        [ResponseType(typeof(ProfileViewModel))]
        public async Task<IActionResult> GetProfileInfo(Guid profileId, CancellationToken cancellationToken)
        {
            var query = new GetUserByIdQuery(profileId);

            Result<UserResponse> response = await _sender.Send(query, cancellationToken);

            return response.IsSuccess ? Ok(response) : BadRequest(response.Error);
        }
    }
}
