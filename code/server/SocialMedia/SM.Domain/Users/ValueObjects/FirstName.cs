namespace SM.Domain.Users.ValueObjects;

public class FirstName
{
    public const int MaxLength = 25;
    public const int MinLength = 1;
    public const string Pattern = "^[\\p{L}'][ \\p{L}'-]*[\\p{L}]$";
}