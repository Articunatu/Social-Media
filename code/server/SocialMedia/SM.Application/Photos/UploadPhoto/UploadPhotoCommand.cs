using Microsoft.AspNetCore.Http;
using SM.Application.Abstractions;

namespace SM.Application.Photos.UploadPhoto;

public record UploadPhotoCommand(IFormFile File) : ICommand<Guid>;
