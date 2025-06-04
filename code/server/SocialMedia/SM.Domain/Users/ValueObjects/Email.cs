
namespace SM.Domain.Users.ValueObjects;

public class Email
{
    public const int MaxLength = 100;
    public const string Pattern = "[^@ \\t\\r\\n]+@[^@ \\t\\r\\n]+\\.[^@ \\t\\r\\n]+";
}
