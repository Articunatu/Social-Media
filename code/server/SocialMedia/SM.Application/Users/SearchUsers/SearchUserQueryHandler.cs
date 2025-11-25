using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Application.Shared.Models;
using SM.Domain.Shared;

namespace SM.Application.Users.SearchUsers;

internal class SearchUserQueryHandler(IDbContextFactory<ApplicationDbContext> contextFactory) : IQueryHandler<SearchUserQuery, IEnumerable<ProfileInfo>>
{
    public async Task<Result<IEnumerable<ProfileInfo>>> Handle(SearchUserQuery request, CancellationToken ct)
    {
        await using var context = await contextFactory.CreateDbContextAsync(ct);

        var search = (request.Filter?.SearchText ?? string.Empty).Trim().ToLower();

        IQueryable<ProfileInfo> query = context.Users.Select(u => u.MapToProfile());

        if (!string.IsNullOrEmpty(search))
        {
            query = context.Users
                .Where(u => u.FirstName.Contains(search, StringComparison.CurrentCultureIgnoreCase) 
                    || u.Tag.Contains(search, StringComparison.CurrentCultureIgnoreCase))
                .Select(u => u.MapToProfile());
        }

        var matchingUsers = await query.ToPagedFeed(request.Filter ?? new PageFilter());

        return Result.Success(matchingUsers.Values);
    }
}
