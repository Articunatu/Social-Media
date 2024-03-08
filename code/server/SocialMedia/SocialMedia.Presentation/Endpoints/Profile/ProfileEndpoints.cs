using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Azure.Cosmos;
using SocialMedia.Application.Users.Commands.AddUser;
using SocialMedia.Application.Users.Queries.GetTop10Users;
using SocialMedia.Application.Users.Queries.GetUserById;
using SocialMedia.Domain.Shared;

namespace SocialMedia.Presentation.Endpoints.Profile
{
    public static class ProfileEndpoints
    {
        public static void MapProfileEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/profiles");

            app.MapGet("{id}", GetProfileInfo);
            app.MapGet("", GetTop10Profiles);
            app.MapPost("", AddNewUser);
        }

        public static async Task<IResult> GetProfileInfo(
            Guid id,
            ISender sender)
        {
            try
            {
                var userResponse = await sender.Send(new GetUserByIdQuery(id));

                return TypedResults.Ok(userResponse);
            }
            catch (Exception e)
            {
                return TypedResults.NotFound(e.Message);
            }
        }

        public static async Task<IResult> GetTop10Profiles(
            int pageNumber,
            ISender sender)
        {
            try
            {
                var usersResponse = await sender.Send(new GetTop10UsersQuery(pageNumber));
                return TypedResults.Ok(usersResponse);
            }
            catch (Exception e)
            {
                return TypedResults.NotFound(e.Message);
            }
        }

        public static async Task<IResult> AddNewUser(
            AddUserCommand request,
            ISender sender)
        {
            var command = new AddUserCommand(
                request.Tag,
                request.Email,
                request.FirstName,
                request.LastName
                );

            await sender.Send(command);

            return Results.Ok();
        }
    }
}
