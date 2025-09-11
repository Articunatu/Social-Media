using MediatR;
using Microsoft.AspNetCore.Mvc;
using SM.Application.Photos.GetPhotoById;
using SM.Application.Photos.GetPhotosByUserId;
using SM.Application.Photos.UploadPhoto;

namespace SM.WebApi.Endpoints;

public static class PhotoEndpoints
{
    public static RouteGroupBuilder MapPhotoEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/photos/");

        group.MapGet("get-photo-by-id/{id}", GetPhotoById);
        group.MapGet("get-photos-by-userid/{userId}", GetPhotosByUserId);
        group.MapPost("upload", UploadPhoto);

        return group;
    }

    public static async Task<IResult> GetPhotoById(Guid id, ISender sender)
    {
        var query = new GetPhotoByIdQuery(id);
        var photo = await sender.Send(query);
        return TypedResults.Ok(photo);
    }

    public static async Task<IResult> GetPhotosByUserId(Guid userId, ISender sender)
    {
        var query = new GetPhotosByUserIdQuery(userId);
        var usersPhotos = await sender.Send(query);
        return TypedResults.Ok(usersPhotos);
    }

    public static async Task<IResult> UploadPhoto([FromBody] UploadPhotoCommand command, ISender sender)
    {
        var uploadResponse = await sender.Send(command);
        return TypedResults.Ok(uploadResponse);
    }
}
