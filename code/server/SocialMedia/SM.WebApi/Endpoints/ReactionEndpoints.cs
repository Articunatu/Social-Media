using MediatR;
using SM.Application.Reactions.GetReactedPostsByUser;
using SM.Application.Reactions.GetReactionsByPost;
using SM.Application.Reactions.RemoveReaction;
using System.Security.Claims;

namespace SM.WebApi.Endpoints;

public static class ReactionEndpoints
{
    public static RouteGroupBuilder MapReactionEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/posts");

        group.MapGet("/post/{postId}/reactions", GetReactionsByPost);
        group.MapGet("/user/{userId}/reactions", GetReactionsByUser);
        group.MapPost("/{id}", ReactToPost);
        group.MapDelete("/{postId}/reactions", RemoveReaction);
        group.MapPut("/{postId}/reactions", UpdateReaction);

        return group;
    }

    public static async Task<IResult> GetReactionsByPost(GetReactionsByPostQuery query, ISender sender)
    {
        var result = await sender.Send(query);
        return TypedResults.Ok(result);
    }

    public static async Task<IResult> GetReactionsByUser(ISender sender, HttpContext context, GetReactedPostsByUserQuery query)
    {
        string? userIdText = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Guid userId = Guid.TryParse(userIdText, out Guid id) ? id : Guid.Empty;

        var authenticatedQuery = new GetReactedPostsByUserQuery(userId, query.PagingIndex);

        var result = await sender.Send(authenticatedQuery);
        return TypedResults.Ok(result);
    }

    public static async Task<IResult> ReactToPost(ISender sender)
    {
        await sender.Send(1);
        return TypedResults.Ok();
    }
    
    public static async Task<IResult> RemoveReaction(RemoveReactionCommand command, ISender sender)
    {
        var result = await sender.Send(command);
        return TypedResults.Ok(result);
    }

    public static async Task<IResult> UpdateReaction(ISender sender)
    {
        await sender.Send(1);
        return TypedResults.Ok();
    }
}
