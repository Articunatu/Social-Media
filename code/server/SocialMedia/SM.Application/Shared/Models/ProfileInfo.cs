using SM.Domain.Photos;

namespace SM.Application.Shared.Models;

public sealed record ProfileInfo(Guid Id, string Tag, string FullName, Photo? ProfilePhoto);

