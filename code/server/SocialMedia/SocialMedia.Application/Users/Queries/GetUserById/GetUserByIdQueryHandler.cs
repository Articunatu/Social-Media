using SocialMedia.Application.Abstractions;
using SocialMedia.Domain.Shared;
using SocialMedia.Domain.Users;

namespace SocialMedia.Application.Users.Queries.GetUserById
{
    internal sealed class GetUserByIdQueryHandler(IUserReadRepository userRepository)
                : IQueryHandler<GetUserByIdQuery, UserResponse>
    {
        readonly IUserReadRepository _userRepository = userRepository;

        public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var query = $"SELECT c.id, c.tag, c.firstName, c.lastName FROM c WHERE {request.Key} = @partitionKey";
            var user = await _userRepository.GetSingle<User>(request.Key, query);
            if (user is null)
                return Result.Failure<UserResponse>(new Error("User.NotFound"));
            var userFullname = user.FirstName + " " + user.LastName;
            var response = new UserResponse(user.Id, user.Tag, userFullname);
            return Result.Success(response);
        }

    }
}
