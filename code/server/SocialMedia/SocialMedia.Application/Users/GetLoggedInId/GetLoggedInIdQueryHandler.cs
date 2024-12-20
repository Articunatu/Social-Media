using SocialMedia.Application.Abstractions;
using SocialMedia.Domain.Shared;
using SocialMedia.Domain.Users.Authentication;

namespace SocialMedia.Application.Users.GetLoggedInId
{
    internal sealed class GetLoggedInIdQueryHandler(IAuthenticationService authenticationService)
        : IQueryHandler<GetLoggedInIdQuery, Guid>
    {
        readonly IAuthenticationService _authenticationService;
        public async Task<Result<Guid>> Handle(GetLoggedInIdQuery request, CancellationToken cancellationToken)
        {
            Guid id = _authenticationService.GetLoggedInUserId();
            if (id == Guid.Empty)
                return Result.Failure<Guid>(new Error("Could not find user's id"));
            return Result.Success(id);
        }
    }
}
