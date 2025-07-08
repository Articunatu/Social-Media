using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Application.Shared.Models;
using SM.Domain.Shared;

namespace SM.Application.Users.SearchUsers;

internal class SearchUserQueryHandler(IDbContextFactory<ApplicationDbContext> contextFactory) : IQueryHandler<SearchUserQuery, IEnumerable<ProfileInfo>>
{
    public async Task<Result<IEnumerable<ProfileInfo>>> Handle(SearchUserQuery request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var search = request.SearchText.ToLower();

        var matchingUsers = await context.Users
            .Where(u =>
                u.FirstName.ToLower().Contains(search) ||
                u.Tag.ToLower().Contains(search))
            .Select(u => u.MapToProfile())
            .ToArrayAsync(cancellationToken);

        return Result.Success<IEnumerable<ProfileInfo>>(matchingUsers);
    }
}
