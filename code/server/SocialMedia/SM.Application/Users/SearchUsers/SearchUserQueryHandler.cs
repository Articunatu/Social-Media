using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Database;
using SM.Application.Shared.Extensions;
using SM.Application.Shared.Models;
using SM.Domain.Shared;

namespace SM.Application.Users.SearchUsers;

internal class SearchUserQueryHandler(ApplicationDbContext context) : IQueryHandler<SearchUserQuery, IEnumerable<ProfileInfo>>
{
    public async Task<Result<IEnumerable<ProfileInfo>>> Handle(SearchUserQuery request, CancellationToken cancellationToken)
    {
        var searchText = request.SearchText.ToLower();

        var matchingUsers = await context.Users
            .Where(u => u.FirstName.Contains(searchText, StringComparison.CurrentCultureIgnoreCase) ||
                        u.Tag.Contains(searchText, StringComparison.CurrentCultureIgnoreCase))
            .Select(u => u.MapToProfile())
            .ToArrayAsync(cancellationToken);

        return Result.Success<IEnumerable<ProfileInfo>>(matchingUsers);
    }
}
