using SocialMedia.Application.Abstractions;
using SocialMedia.Domain.Shared;
using SocialMedia.Domain.Users.Authentication;

namespace SocialMedia.Application.Users.GetLoginTag
{
    internal sealed class GetLoginTagQueryHandler(IAuthenticationService authenticationService)
        : IQueryHandler<GetLoginTagQuery, string>
    {
        readonly IAuthenticationService _authenticationService;

        public async Task<Result<string>> Handle(GetLoginTagQuery request, CancellationToken cancellationToken)
        {
            string tag = _authenticationService.GetLoginTag();
            if (tag is null)
                return Result.Failure<string>(new Error("Could not find user's tag"));
            return Result.Success(tag);
        }
    }
}
