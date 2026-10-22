using MediatR;
using Microsoft.AspNetCore.Mvc;
using SM.Application.Messaging.SendMessage;
using SM.WebApi.Extensions;

namespace SM.WebApi.Endpoints;

public static class MessagingEndpoints
{
    public static RouteGroupBuilder MapMessagingEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/messages/");

        group.MapPost("send", SendMessage).RequireAuthorization();

        return group;
    }

    public static async Task<IResult> SendMessage(
        [FromBody] SendMessageCommand command,
        ISender sender,
        HttpContext httpContext)
    {
        var authorId = httpContext.GetLoggedInUserId();
        if (authorId == Guid.Empty)
            return TypedResults.Unauthorized();

        var result = await sender.Send(command with { AuthorId = authorId });
        return result.ToActionResult();
    }
}