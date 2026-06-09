using SM.Application.Abstractions;

namespace SM.Application.Photos.GetPhotosByUserId;

public record GetPhotosByUserIdQuery(Guid UserId) : IQuery<IEnumerable<PhotoResponse>>;
