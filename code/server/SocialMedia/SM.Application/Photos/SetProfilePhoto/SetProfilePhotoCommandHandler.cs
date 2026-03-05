using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Domain.Photos;
using SM.Domain.Shared;
using System.Net;

namespace SM.Application.Photos.SetProfilePhoto;

internal class SetProfilePhotoCommandHandler(IDbContextFactory<ApplicationDbContext> contextFactory) : ICommandHandler<SetProfilePhotoCommand, bool>
{
    public async Task<Result<bool>> Handle(SetProfilePhotoCommand request, CancellationToken ct)
    {
        await using var context = await contextFactory.CreateDbContextAsync(ct);

        var photoToEdit = await context.Photos
            .FirstOrDefaultAsync(p => p.Id == request.PhotoId && p.UserId == request.UserId, ct);

        if (photoToEdit is null) 
            return Result.Failure<bool>(new Error("Photo.NotFound"), HttpStatusCode.NotFound);

        photoToEdit.Type = PhotoType.Profile;

        await context.SaveChangesAsync(ct);

        return Result.Success(true);
    }
}
