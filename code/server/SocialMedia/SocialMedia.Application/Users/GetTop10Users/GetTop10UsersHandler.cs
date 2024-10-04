using SocialMedia.Application.Abstractions;
using SocialMedia.Domain.Shared;
using SocialMedia.Domain.Users;


namespace SocialMedia.Application.Users.GetTop10Users
{
    internal sealed class GetTop10UsersQueryHandler(IUserRepository userRepository)
                : IQueryHandler<GetTop10UsersQuery, IEnumerable<UsersResponse>>
    {
        readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<IEnumerable<UsersResponse>>> Handle(GetTop10UsersQuery request, CancellationToken cancellationToken)
        {
            int pageNumber = request.PageNumber;
            int pageSize = 10;

            int skip = (pageNumber - 1) * pageSize;

            var query = $"SELECT c.id, c.tag, c.firstName, c.lastName FROM c OFFSET {skip} LIMIT {pageSize}";

            var users = await _userRepository.GetMultiple<User>(0, query);

            if (users == null)
            {
                return Result.Failure<IEnumerable<UsersResponse>>(new Error("10_Users.NotFound"));
            }

            var userResponses = users.Select(user =>
            {
                string userFullname = user.FirstName + " " + user.LastName;
                return new UsersResponse(user.Id, user.Tag, userFullname);
            }).ToList();

            return Result.Success((IEnumerable<UsersResponse>)userResponses);
        }
    }
}
