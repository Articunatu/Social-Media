using SocialMedia.Application.Abstractions;
using SocialMedia.Application.Users.Queries.Application.Users.Queries.GetUserById;
using SocialMedia.Domain.Shared;
using SocialMedia.Domain.Users;
//f6e9ba5c-222d-45b1-98e6-f30adfc70940 meigour
//62a2098a-95c3-4e29-ae78-2748b90fa50b artciunatu

namespace SocialMedia.Application.Users.Queries.GetUserById
{
    internal sealed class GetUserByIdQueryHandler(IUserReadRepository userRepository)
                : IQueryHandler<GetUserByIdQuery, UserResponse>
    {
        readonly IUserReadRepository _userRepository = userRepository;

        public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var query = "SELECT c.id, c.Tag, c.FirstName, c.LastName FROM c WHERE c.id=@partitionKey";
            var user = await _userRepository.GetSingle<User>(request.Key, query);
            if (user is null || user.Tag is null)
                return Result.Failure<UserResponse>(new Error("User.NotFound"));
            string userFullname = user.FirstName + " " + user.LastName;
            var response = new UserResponse(user.Id, user.Tag.ToString(), userFullname);
            return Result.Success(response);
        }
    }
}
