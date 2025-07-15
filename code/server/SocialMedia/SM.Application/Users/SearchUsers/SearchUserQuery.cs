using SM.Application.Abstractions;
using SM.Application.Shared.Models;

namespace SM.Application.Users.SearchUsers;

public record SearchUserQuery(PageFilter Filter) : IQuery<IEnumerable<ProfileInfo>>;