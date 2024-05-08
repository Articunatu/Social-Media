using MediatR;
using SocialMedia.Application.Users.AddUser;
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

            app.MapPost("register", Register);
            app.MapGet("login", LogIn);
            app.MapGet("id", GetLoggedInUserId);
            //app.MapPost("", RefreshToken);
        }
        public static async Task<IResult> Register(
            AddUserCommand request,
            ISender sender,
            CancellationToken cancellationToken)
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

        public static async Task<IResult> LogIn(
            LogInUserRequest request,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var command = new LogInUserCommand(request.Email, request.Password);

            var result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
                return TypedResults.BadRequest(result.Error);

            return TypedResults.Ok(result.Value);
        }

        public static async Task<IResult> GetLoggedInUserId(ISender sender)
        {
            var userIdResponse = await sender.Send(new GetLoggedInIdQuery());

            if(userIdResponse.IsFailure)
                return TypedResults.BadRequest(userIdResponse.Error);

            return TypedResults.Ok(userIdResponse);
        }

        //public static async Task<IResult> RefreshToken(ISender sender)
        //{
        //    return sender.Send(RefreshTokenCommand);
        //}
    }
}