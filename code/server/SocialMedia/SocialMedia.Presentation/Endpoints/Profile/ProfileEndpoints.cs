using MediatR;
using SocialMedia.Application.Users.GetTop10Users;
using SocialMedia.Application.Users.GetUserById;
using SocialMedia.Domain.Abstractions;

namespace SocialMedia.Presentation.Endpoints.Profile
{
    public static class ProfileEndpoints
    {
        public static void MapProfileEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/profiles");

            app.MapGet("getprofile{id}", GetProfileInfo);
            app.MapGet("get10profiles", GetTop10Profiles);
        }

        public static async Task<IResult> GetProfileInfo(
            Guid id,
            ISender sender)
        {
            try
            {
                var userResponse = await sender.Send(new GetUserByIdQuery(id, KeyEnum.Id));
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
    }
}
