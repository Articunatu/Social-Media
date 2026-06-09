using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Domain.Photos;
using SM.Domain.Shared;

namespace SM.Application.Photos.GetPhotosByUserId;

internal class GetPhotosByUserIdQueryHandler(IDbContextFactory<ApplicationDbContext> contextFactory)
    : IQueryHandler<GetPhotosByUserIdQuery, IEnumerable<PhotoResponse>>
{
    public async Task<Result<IEnumerable<PhotoResponse>>> Handle(GetPhotosByUserIdQuery request, CancellationToken ct)
    {
        await using var context = await contextFactory.CreateDbContextAsync(ct);

        var photos = await context.Photos
            .Where(p => p.UserId == request.UserId)
            .OrderByDescending(p => p.Type == PhotoType.Profile)
            .ThenByDescending(p => p.CreatedAt)
            .ToListAsync(ct);

        return Result.Success<IEnumerable<PhotoResponse>>(photos.Select(p => p.MapToResponse()));
    }
}
