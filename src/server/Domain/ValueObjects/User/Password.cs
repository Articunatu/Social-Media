using Domain.Primitives;
using Domain.Shared;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects.User
{
    public class Password : ValueObject
    {
        public string Value { get; }
        public const string RegexPattern = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$";
        static Regex Regex = new(RegexPattern);

        public Password(string value)
        {
            Value = value;
        }

        public static Result<Password> Create(string password)
        {
            if (Regex.IsMatch(password))
            {
                return Result.Failure<Password>(new Error(
                    "Password.Weak",
                    "The password is either not long enough, lacks both numbers, letters and special characters."
                    ));
            }
            return new Password(password);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
