using MediatR;
using SM.Application.Reactions.AddReaction;
using SM.Application.Reactions.GetReactedPostsByUser;
using SM.Application.Reactions.GetReactionsByPost;
using SM.Application.Reactions.RemoveReaction;
using SM.Application.Reactions.UpdateReaction;
using SM.WebApi.Extensions;

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
        var reactions = await sender.Send(query);
        return TypedResults.Ok(reactions);
    }

    public static async Task<IResult> GetReactionsByUser(ISender sender, HttpContext context, GetReactedPostsByUserQuery query)
    {
        Guid userId = context.GetLoggedInUserId();

        var authenticatedQuery = new GetReactedPostsByUserQuery(userId, query.PagingIndex);

        var reactedPosts = await sender.Send(authenticatedQuery);
        return TypedResults.Ok(reactedPosts);
    }

    public static async Task<IResult> ReactToPost(AddReactionCommand command, ISender sender)
    {
        var reactedPost = await sender.Send(command);
        return TypedResults.Ok(reactedPost);
    }
    
    public static async Task<IResult> RemoveReaction(RemoveReactionCommand command, ISender sender)
    {
        var removedReaction = await sender.Send(command);
        return TypedResults.Ok(removedReaction);
    }

    public static async Task<IResult> UpdateReaction(UpdateReactionCommand command, ISender sender)
    {
        var updatedReaction = await sender.Send(command);
        return TypedResults.Ok(updatedReaction);
    }
}
