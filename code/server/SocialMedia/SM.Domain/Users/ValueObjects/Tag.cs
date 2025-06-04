
namespace SM.Domain.Users.ValueObjects;

public class Tag
{
    public const int MaxLength = 20;
    public const string Pattern = "^[a-z0-9_-]{2,20}$";
}
