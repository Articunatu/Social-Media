using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Domain.Shared;
using System.Net;

namespace SM.Application.Photos.GetPhotoById;

internal class GetPhotoByIdQueryHandler(IDbContextFactory<MediaDbContext> contextFactory)
    : IQueryHandler<GetPhotoByIdQuery, PhotoResponse>
{
    public async Task<Result<PhotoResponse>> Handle(GetPhotoByIdQuery request, CancellationToken ct)
    {
        await using var context = await contextFactory.CreateDbContextAsync(ct);

        var photo = await context.Photos.FirstOrDefaultAsync(p => p.Id == request.Id, ct);

        if (photo is null)
            return Result.Failure<PhotoResponse>(new Error("PhotoMissing"), HttpStatusCode.NotFound);

        return Result.Success(photo.MapToResponse());
    }
}
