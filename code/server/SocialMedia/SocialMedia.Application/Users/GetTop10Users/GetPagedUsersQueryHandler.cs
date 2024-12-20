using SocialMedia.Application.Abstractions;
using SocialMedia.Domain.Shared;
using SocialMedia.Domain.Users;


namespace SocialMedia.Application.Users.GetTop10Users
{
    internal sealed class GetPagedUsersQueryHandler(IUserNoSqlRepository userRepository)
                : IQueryHandler<GetPagedUsersQuery, IEnumerable<PagedUsersResponse>>
    {
        readonly IUserNoSqlRepository _userRepository = userRepository;

        public async Task<Result<IEnumerable<PagedUsersResponse>>> Handle(GetPagedUsersQuery request, CancellationToken cancellationToken)
        {
            int pageNumber = request.PageNumber;
            const int PAGE_SIZE = 10; 
            int skip = (pageNumber - 1) * PAGE_SIZE;

            var users = await _userRepository.GetMultiple<(Guid Id, string FirstName, string LastName, string Tag)>(
                queryModifier: query => (IQueryable<UserNoSql>)query
                    .Skip(skip) 
                    .Take(PAGE_SIZE) 
                    .Select(u => new { u.Id, u.FirstName, u.LastName, u.Email }) 
            );

            if (users is null)
                return Result.Failure<IEnumerable<PagedUsersResponse>>(new Error("10_Users.NotFound"));

            var userResponses = users.Select(user =>
            {
                string userFullname = user.FirstName + " " + user.LastName;
                return new PagedUsersResponse(user.Id, user.Tag, userFullname);
            }).ToArray();

            return Result.Success((IEnumerable<PagedUsersResponse>)userResponses);
        }
    }
}
