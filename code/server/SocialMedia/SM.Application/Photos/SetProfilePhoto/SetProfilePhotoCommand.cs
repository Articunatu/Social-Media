using SM.Application.Abstractions;

namespace SM.Application.Photos.SetProfilePhoto;

public record SetProfilePhotoCommand(Guid UserId, Guid PhotoId) : ICommand<bool>;
