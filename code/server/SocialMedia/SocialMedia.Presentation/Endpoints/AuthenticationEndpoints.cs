using MediatR;
using SocialMedia.Application.Users.Commands.AddUserCommand;
using SocialMedia.Application.Users.Commands.LogInUser;
using SocialMedia.Application.Users.Queries.GetLoggedInId;
using SocialMedia.Presentation.Endpoints.Profile;

namespace SocialMedia.Presentation.Endpoints.Authentication
{
    public static class AuthenticationEndpoints
    {
        public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/profiles");

            app.MapPost("{request}", SignUp);
            app.MapGet("{request}", LoginAsync);
            app.MapGet("id", GetLoggedInUserId);
            app.MapPost("", RefreshToken);
        }

        public static async Task<IResult> SignUp(
            AddUserRequest request,
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
                return TypedResults.BadRequest(result.Error);

            return TypedResults.Ok(result.Value);
        }

        public static async Task<IResult> LoginAsync(
            LogInUserRequest request,
            ISender sender)
        {
            var loginResponse = await sender.Send(new LogInUserCommand(request.Email, request.Password));

            if (loginResponse.IsFailure)
                return TypedResults.BadRequest(loginResponse.Error); 

            return TypedResults.Ok(loginResponse);
        }

        public static async Task<IResult> GetLoggedInUserId(ISender sender)
        {
            var userIdResponse = await sender.Send(new GetLoggedInIdQuery());
            if(userIdResponse.IsFailure)
                return TypedResults.BadRequest(userIdResponse.Error);
            return TypedResults.Ok(userIdResponse);
        }

        public static async Task<IResult> RefreshToken()
        {
            return TypedResults.BadRequest();
        }
    }
}