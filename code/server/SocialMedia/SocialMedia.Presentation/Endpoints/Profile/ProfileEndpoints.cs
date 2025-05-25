using MediatR;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Application.Profiles.GetProfilePosts;

namespace SocialMedia.Presentation.Endpoints.Profile;

public static class ProfileEndpoints
{
    public static void MapProfileEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/profiles");

        app.MapPost("getprofilefeed", GetProfileFeed);
    }

    public static async Task<IResult> GetProfileFeed([FromBody] GetProfilePostsQuery request, ISender sender)
    {
        try
        {
            var usersResponse = await sender.Send(new GetProfilePostsQuery(request.UserId, request.Filter));
            return TypedResults.Ok(usersResponse);
        }
        catch (Exception e)
        {
            return TypedResults.NotFound(e.Message);
        }
    }
}