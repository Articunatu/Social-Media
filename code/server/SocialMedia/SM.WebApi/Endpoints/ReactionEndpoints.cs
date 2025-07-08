using MediatR;
using Microsoft.AspNetCore.Mvc;
using SM.Application.Reactions.AddReaction;
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
        var group = routes.MapGroup("/api");

        group.MapGet("/post/{postId}/reactions", GetReactionsByPost);
        group.MapGet("/user/reactions", GetReactionsByUser);
        group.MapPost("/reactions", ReactToPost);
        group.MapDelete("/reactions", RemoveReaction);
        group.MapPut("/reactions", UpdateReaction);

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


    public static async Task<IResult> GetReactionsByUser(ISender sender, HttpContext context,
        [AsParameters] PageFilter filter)
    {
        Guid userId = context.GetLoggedInUserId();

        var authenticatedQuery = new GetReactedPostsByUserQuery(userId, filter.Index);

        var reactedPosts = await sender.Send(authenticatedQuery);
        return TypedResults.Ok(reactedPosts);
    }

    public static async Task<IResult> ReactToPost([FromBody] AddReactionCommand command, ISender sender)
    {
        var reactedPost = await sender.Send(command);
        return TypedResults.Ok(reactedPost);
    }
    
    public static async Task<IResult> RemoveReaction([FromBody] RemoveReactionCommand command, ISender sender)
    {
        var removedReaction = await sender.Send(command);
        return TypedResults.Ok(removedReaction);
    }

    public static async Task<IResult> UpdateReaction([FromBody] UpdateReactionCommand command, ISender sender)
    {
        var updatedReaction = await sender.Send(command);
        return TypedResults.Ok(updatedReaction);
    }
}
