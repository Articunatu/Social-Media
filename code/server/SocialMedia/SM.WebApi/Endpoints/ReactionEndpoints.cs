using MediatR;
using SM.Application.Reactions.GetReactionsByPost;

namespace SM.WebApi.Endpoints;

public static class ReactionEndpoints
{
    public static RouteGroupBuilder MapReactionEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/posts");

        group.MapGet("/{postId}/reactions", GetReactionsByPost);
        group.MapGet("/{userId}/reactions", GetReactionsByUser);
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

    private static async Task GetReactionsByUser(HttpContext context)
    {
        throw new NotImplementedException();
    }

    public static async Task<IResult> ReactToPost(ISender sender)
    {
        await sender.Send(1);
        return TypedResults.Ok();
    }
    
    public static async Task<IResult> RemoveReaction(ISender sender)
    {
        await sender.Send(1);
        return TypedResults.Ok();
    }

    public static async Task<IResult> UpdateReaction(ISender sender)
    {
        await sender.Send(1);
        return TypedResults.Ok();
    }
}
