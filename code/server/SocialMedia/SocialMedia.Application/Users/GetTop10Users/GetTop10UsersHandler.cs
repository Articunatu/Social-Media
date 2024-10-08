using SocialMedia.Application.Abstractions;
using SocialMedia.Domain.Shared;
using SocialMedia.Domain.Users;


namespace SocialMedia.Application.Users.GetTop10Users
{
    internal sealed class GetTop10UsersQueryHandler(IUserNoSqlRepository userRepository)
                : IQueryHandler<GetTop10UsersQuery, IEnumerable<UsersResponse>>
    {
        readonly IUserNoSqlRepository _userRepository = userRepository;

        public async Task<Result<IEnumerable<UsersResponse>>> Handle(GetTop10UsersQuery request, CancellationToken cancellationToken)
        {
            int pageNumber = request.PageNumber;
            int pageSize = 10; // You can also make this configurable
            int skip = (pageNumber - 1) * pageSize;

            var users = await _userRepository.GetMultiple<(Guid Id, string FirstName, string LastName, string Tag)>(
                queryModifier: query => (IQueryable<UserNoSql>)query
                    .Skip(skip) // Apply the skip for paging
                    .Take(pageSize) // Limit the number of items returned
                    .Select(u => new { u.Id, u.FirstName, u.LastName, u.Email }) // Select specific fields
            );


            if (users is null)
                return Result.Failure<IEnumerable<UsersResponse>>(new Error("10_Users.NotFound"));

            var userResponses = users.Select(user =>
            {
                string userFullname = user.FirstName + " " + user.LastName;
                return new UsersResponse(user.Id, user.Tag, userFullname);
            }).ToArray();

            return Result.Success((IEnumerable<UsersResponse>)userResponses);
        }
    }
}
