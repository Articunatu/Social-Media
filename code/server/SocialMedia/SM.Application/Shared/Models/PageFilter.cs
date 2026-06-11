
namespace SM.Application.Shared.Models;

public record PageFilter
{
    public int Index { get; init; }
    public string? Order { get; init; } = string.Empty;
    public string? SearchText { get; init; } = string.Empty;
}
