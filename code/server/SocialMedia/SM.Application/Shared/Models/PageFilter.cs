
namespace SM.Application.Shared.Models;

public record PageFilter
{
    public int Index { get; set; }
    public string Order { get; set; } = string.Empty;
    public string SearchText { get; init; } = string.Empty;
}
