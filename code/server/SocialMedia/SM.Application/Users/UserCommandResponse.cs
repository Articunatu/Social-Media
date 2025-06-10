
namespace SM.Application.Users;

public record UserCommandResponse
{
    public Guid Id { get; set; }
    public string Tag { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
