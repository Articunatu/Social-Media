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

        await context.Photos
            .Where(p => p.UserId == request.UserId && p.Type == PhotoType.Profile)
            .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.Type, PhotoType.Regular), ct);

        var affectedRows = await context.Photos
            .Where(p => p.Id == request.PhotoId && p.UserId == request.UserId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.Type, PhotoType.Profile), ct);

        if (affectedRows == 0)
            return Result.Failure<bool>(new Error("Photo.NotFound"), HttpStatusCode.NotFound);

        return Result.Success(true);
    }
}
