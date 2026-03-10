
namespace SM.Application.Users;

public record UserCommandResponse
{
    public Guid Id { get; init; }
    public string Tag { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}
