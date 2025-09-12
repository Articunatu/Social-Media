using SM.Application.Abstractions;

namespace SM.Application.Photos.GetPhotoById;

public record GetPhotoByIdQuery(Guid Id) : IQuery<PhotoResponse>;
