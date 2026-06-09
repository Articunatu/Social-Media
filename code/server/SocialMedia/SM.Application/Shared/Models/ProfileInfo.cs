using SM.Application.Photos;

namespace SM.Application.Shared.Models;

public sealed record ProfileInfo(Guid Id, string Tag, string FullName, PhotoResponse? ProfilePhoto);
