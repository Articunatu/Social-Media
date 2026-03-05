using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Photos.UploadPhoto.Extensions;
using SM.Domain.Photos;
using SM.Domain.Shared;
using System.Net;

namespace SM.Application.Photos.UploadPhoto;

internal class UploadPhotoCommandHandler(IDbContextFactory<ApplicationDbContext> contextFactory) : ICommandHandler<UploadPhotoCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UploadPhotoCommand request, CancellationToken ct)
    {
        long maxFileSizeBytes = 5 * 1024 * 1024;

        var fileInfo = request.File;

        if (fileInfo is null || !fileInfo.Exists)
            return Result.Failure<Guid>(new Error("File does not exist."), HttpStatusCode.BadRequest);

        if (fileInfo.Length > maxFileSizeBytes)
            return Result.Failure<Guid>(
                new Error($"File exceeds the maximum allowed size of {maxFileSizeBytes / (1024 * 1024)} MB."),
                HttpStatusCode.BadRequest
            );

        var data = await File.ReadAllBytesAsync(fileInfo.FullName, ct);

        var contentType = fileInfo.Name.GetContentType();

        if (contentType is null)
            return Result.Failure<Guid>(new Error("Unsupported file type."), HttpStatusCode.UnsupportedMediaType);

        var photo = new Photo(Guid.CreateVersion7())
        {
            FileName = fileInfo.Name,
            ContentType = contentType,
            Data = data,
            UserId = request.UserId,
            Type = PhotoType.Regular
           
        };

        await using var context = await contextFactory.CreateDbContextAsync(ct);

        context.Photos.Add(photo);
        await context.SaveChangesAsync(ct);

        return Result.Success(photo.Id);
    }
}
