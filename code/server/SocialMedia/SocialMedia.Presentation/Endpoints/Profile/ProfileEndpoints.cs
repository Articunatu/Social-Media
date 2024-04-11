using MediatR;
using SocialMedia.Application.Users.Commands.AddUserCommand;
using SocialMedia.Application.Users.Commands.LogInUser;
using SocialMedia.Application.Users.Queries.GetTop10Users;
using SocialMedia.Application.Users.Queries.GetUserById;
using SocialMedia.Domain.Abstractions;

namespace SocialMedia.Presentation.Endpoints.Profile
{
    public static class ProfileEndpoints
    {
        public static void MapProfileEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/profiles");

            app.MapGet("{id}", GetProfileInfo);
            app.MapGet("", GetTop10Profiles);
            app.MapPost("register", Register);
            app.MapPost("", LogIn);
        }

        public static async Task<IResult> GetProfileInfo(
            Guid id,
            ISender sender)
        {
            try
            {
                var userResponse = await sender.Send(new GetUserByIdQuery(id, KeyTypes.Id));
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

        public static async Task<IResult> Register(
            AddUserCommand request,
            CancellationToken cancellationToken,
            ISender sender)
        {
            var command = new AddUserCommand(
                request.Tag,
                request.Email,
                request.FirstName,
                request.LastName,
                request.Password
                );

            var result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return TypedResults.BadRequest(result.Error);
            }

            return TypedResults.Ok(result.Value);
        }

        public static async Task<IResult> LogIn(
            LogInUserRequest request,
            CancellationToken cancellationToken,
            ISender sender)
        {
            var command = new LogInUserCommand(request.Email, request.Password);

            var result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return TypedResults.BadRequest(result.Error);
            }

            return TypedResults.Ok(result.Value);
        }
    }
}
