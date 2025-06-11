using SM.Application.Shared.Models;
using SM.Domain.Photos;

namespace SM.Application.Users.GetProfile;

public record ProfileDetails
{
    public ProfileInfo Profile { get; set; } = default!;
    public string AboutMe { get; set; } = string.Empty;
    public int FollowingCount { get; set; }
    public int FollowersCount { get; set; }
    public Photo? BackgroundPhoto { get; set; }
}
