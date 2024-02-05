

namespace SocialMedia.Presentation.Endpoints.Profile
{
    public sealed record AddUserRequest(
        string Tag,
        string Email,
        string FirstName,
        string LastName)
    { }
}
