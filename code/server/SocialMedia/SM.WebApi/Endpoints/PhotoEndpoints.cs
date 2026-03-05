using MediatR;
using Microsoft.AspNetCore.Mvc;
using SM.Application.Photos;
using SM.Application.Photos.GetPhotoById;
using SM.Application.Photos.GetPhotosByUserId;
using SM.Application.Photos.SetProfilePhoto;
using SM.Application.Photos.UploadPhoto;

namespace SM.WebApi.Endpoints;

public static class PhotoEndpoints
{
    public static RouteGroupBuilder MapPhotoEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/photos/");

        group.MapGet("get-photo-by-id/{id}", GetPhotoById);
        group.MapGet("get-photos-by-userid/{userId}", GetPhotosByUserId);
        //group.MapPost("/{userId}/upload", UploadPhoto); ///Fails in Swagger
        group.MapPatch("set-pfp", SetProfilePictureByPhotoAndUserId).RequireAuthorization();

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

    //public static async Task<IResult> UploadPhoto([FromRoute] Guid userId, [FromForm] IFormFile file, ISender sender)
    //{
    //    var tempFilePath = Path.GetTempFileName();
    //    using (var stream = File.Create(tempFilePath))
    //    {
    //        await file.CopyToAsync(stream);
    //    }
    //    var fileInfo = new FileInfo(tempFilePath);
    //    var command = new UploadPhotoCommand(fileInfo, userId);

    //    var uploadResponse = await sender.Send(command);

    //    File.Delete(tempFilePath);
     
    //    return TypedResults.Ok(uploadResponse);
    //}

    public static async Task<IResult> SetProfilePictureByPhotoAndUserId([FromBody] SetProfilePhotoCommand command, ISender sender)
    {
        var isUpdated = await sender.Send(command);
        return TypedResults.Ok(isUpdated);
    }
}
