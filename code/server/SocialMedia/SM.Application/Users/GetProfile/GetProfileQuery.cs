using SM.Application.Abstractions;

namespace SM.Application.Users.GetProfile;

public record GetProfileQuery(Guid Id, Guid ViewerId) : IQuery<ProfileDetails>;
