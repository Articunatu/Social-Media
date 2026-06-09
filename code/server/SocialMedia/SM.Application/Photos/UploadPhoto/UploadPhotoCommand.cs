using SM.Application.Abstractions;

namespace SM.Application.Photos.UploadPhoto;

public record UploadPhotoCommand(FileInfo File, string FileName, Guid UserId) : ICommand<Guid>;
