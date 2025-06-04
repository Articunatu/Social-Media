
namespace SM.Domain.Users.ValueObjects;

public class Password
{
    public const int MinLength = 8;
    public const int MaxLength = 99;
    public const string Pattern = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*\\-+]).{8,}$";
}
