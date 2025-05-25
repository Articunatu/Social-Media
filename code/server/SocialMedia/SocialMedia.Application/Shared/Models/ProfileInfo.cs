
using SocialMedia.Domain.Photos;

namespace SocialMedia.Application.Shared.Models;

public sealed record ProfileInfo(Guid Id, string Tag, string FullName, Photo? ProfilePhoto);

