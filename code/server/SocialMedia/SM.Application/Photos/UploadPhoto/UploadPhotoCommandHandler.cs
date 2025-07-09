using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Domain.Photos;
using SM.Domain.Shared;

namespace SM.Application.Photos.UploadPhoto;

internal class UploadPhotoCommandHandler(IDbContextFactory<ApplicationDbContext> contextFactory) 
    : ICommandHandler<UploadPhotoCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UploadPhotoCommand request, CancellationToken cancellationToken)
    {
        var file = request.File;

        if (file is null || file.Length == 0)
            return Result.Failure<Guid>(new Error("No file uploaded."), StatusCode.Validation);

        using var ms = new MemoryStream();
        file.CopyTo(ms);
        var data = ms.ToArray();

        var photo = new Photo(Guid.CreateVersion7())
        {
            FileName = file.FileName,
            ContentType = file.ContentType,
            Data = data
        };

        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        context.Photos.Add(photo);

        return Result.Success(photo.Id);
    }
}
