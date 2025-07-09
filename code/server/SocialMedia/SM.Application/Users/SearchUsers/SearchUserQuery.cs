using SM.Application.Abstractions;
using SM.Application.Shared.Models;

namespace SM.Application.Users.SearchUsers;

public record SearchUserQuery(string SearchText, PageFilter Filter) : IQuery<IEnumerable<ProfileInfo>>;