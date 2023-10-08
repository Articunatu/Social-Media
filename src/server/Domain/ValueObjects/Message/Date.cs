using Domain.Primitives;
using Domain.Shared;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects.Message
{
    public class Date : ValueObject
    {
        public string Value { get; }

        public const string RegexPattern = "^(?:(?:(31[\\/\\-\\.](0?[13578]|1[02]))\\2|(?:(29|30)[\\/\\-\\.](0?[13-9]|1[0-2])\\4))((1[6-9]|[2-9]\\d)?\\d{2})$|^(29[\\/\\-\\.]0?2\\6(?:(?:(1[6-9]|[2-9]\\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))$|^(0?[1-9]|1\\d|2[0-8])[\\/\\-\\.]((0?[1-9])|(1[0-2]))\\9((1[6-9]|[2-9]\\d)?\\d{2})$\r\n";
        static Regex Regex = new(RegexPattern);

        public Date(string value)
        {
            Value = value;
        }

        public static Result<Date> Create(string date)
        {
            if (!Regex.IsMatch(date))
            {
                return Result.Failure<Date>(new Error(
                    "Date.FormatInvalid",
                    "The date is either not a real one, the numbers aren't separated by -, / or ."));
            }
            return new Date(date);
        }
        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
