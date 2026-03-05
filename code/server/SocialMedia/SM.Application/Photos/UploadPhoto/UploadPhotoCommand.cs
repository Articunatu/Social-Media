using SM.Application.Abstractions;

namespace SM.Application.Photos.UploadPhoto;

public record UploadPhotoCommand(FileInfo File, Guid UserId) : ICommand<Guid>;
