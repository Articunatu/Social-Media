//using SocialMedia.Application.Abstractions;
//using SocialMedia.Domain.Shared;
//using SocialMedia.Domain.Users;

//namespace SocialMedia.Application.Users.Queries.GetTop10Users
//{
//    internal sealed class GetUserByIdQueryHandler(IUserReadRepository userRepository)
//                : IQueryHandler<GetTop10UsersQuery, UserResponse>
//    {
//        readonly IUserReadRepository _userRepository = userRepository;

//        public async Task<Result<UserResponse>> Handle(GetTop10UsersQuery request, CancellationToken cancellationToken)
//        {
//            var query = "SELECT TOP 10 * FROM c";
//            var user = await _userRepository.GetMultiple<User>(request.userId, query);

//            if (user is null)
//            {
//                return Result.Failure<UserResponse>(new Error(
//                    "User.NotFound"));
//            }
//            string userFullname = user.FirstName + " " + user.LastName;
//            var response = new UserResponse(user.Id, user.Tag, userFullname);

//            return Result<UserResponse>.Success(response);
//        }

//    }
//}
