using MediatR;
using Microsoft.AspNetCore.Mvc;
using SM.Application.Reactions.AddReaction;
using SM.Application.Reactions.GetMyReactionByPost;
using SM.Application.Reactions.GetReactedPostsByUser;
using SM.Application.Reactions.GetReactionsByPost;
using SM.Application.Reactions.RemoveReaction;
using SM.Application.Reactions.UpdateReaction;
using SM.Application.Shared.Models;
using SM.Domain.Reactions;
using SM.WebApi.Extensions;

namespace SM.WebApi.Endpoints;

public static class ReactionEndpoints
{
    public static RouteGroupBuilder MapReactionEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/reactions/");

        group.MapGet("get-by-post/{postId}", GetReactionsByPost);
        group.MapGet("my-reaction/{postId}", GetMyReactionByPost).RequireAuthorization();
        group.MapGet("users-reactions/{userId}", GetReactionsByUser);
        group.MapPost("react-to-post", ReactToPost).RequireAuthorization();
        group.MapDelete("delete/{id}", RemoveReaction).RequireAuthorization();
        group.MapPatch("update-reaction", UpdateReaction).RequireAuthorization();

        return group;
    }

    public static async Task<IResult> GetReactionsByPost(
        Guid postId,
        ReactionType? type,
        int pageNumber,
        ISender sender)
    {
        var filter = new PageFilter { Index = pageNumber};
        var query = new GetReactionsByPostQuery(postId, type, filter);
        var result = await sender.Send(query);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.BadRequest(result.Error);
    }

    public static async Task<IResult> GetReactionsByUser(ISender sender, Guid userId, [AsParameters] PageFilter filter)
    {
        var authenticatedQuery = new GetReactedPostsByUserQuery(userId, filter.Index);

        var reactedPosts = await sender.Send(authenticatedQuery);
        return reactedPosts.ToActionResult();
    }

    public static async Task<IResult> GetMyReactionByPost(Guid postId, ISender sender, HttpContext httpContext)
    {
        var userId = httpContext.GetLoggedInUserId();
        if (userId == Guid.Empty)
            return TypedResults.Unauthorized();

        var result = await sender.Send(new GetMyReactionByPostQuery(userId, postId));
        if (result.IsSuccess)
            return TypedResults.Ok(result.Value);

        return result.Status == System.Net.HttpStatusCode.NotFound
            ? TypedResults.NoContent()
            : TypedResults.BadRequest(result.Error);
    }

    public static async Task<IResult> ReactToPost([FromBody] AddReactionCommand command, ISender sender, HttpContext httpContext)
    {
        var userId = httpContext.GetLoggedInUserId();
        if (userId == Guid.Empty)
            return TypedResults.Unauthorized();

        command = command with { UserId = userId };
        var reactedPost = await sender.Send(command);
        return reactedPost.ToActionResult();
    }
    
    public static async Task<IResult> RemoveReaction(Guid id, ISender sender, HttpContext httpContext)
    {
        var userId = httpContext.GetLoggedInUserId();
        if (userId == Guid.Empty)
            return TypedResults.Unauthorized();

        var removedReaction = await sender.Send(new RemoveReactionCommand(id));
        return removedReaction.ToActionResult();
    }

    public static async Task<IResult> UpdateReaction([FromBody] UpdateReactionCommand command, ISender sender, HttpContext httpContext)
    {
        var userId = httpContext.GetLoggedInUserId();
        if (userId == Guid.Empty)
            return TypedResults.Unauthorized();

        var updatedReaction = await sender.Send(command);
        return updatedReaction.ToActionResult();
    }
}
