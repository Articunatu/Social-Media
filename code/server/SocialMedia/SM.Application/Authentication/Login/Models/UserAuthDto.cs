
namespace SM.Application.Authentication.Login.Models;

public record UserAuthDto(Guid Id, byte[] PasswordHash, byte[] PasswordSalt);