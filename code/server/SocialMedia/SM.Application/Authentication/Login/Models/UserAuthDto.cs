
namespace SM.Application.Authentication.Login.Models;

public record UserAuthDto(Guid Id, string Tag, byte[] PasswordHash, byte[] PasswordSalt);