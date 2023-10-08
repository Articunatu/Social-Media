using Domain.Primitives;
using Domain.Shared;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects.User
{
    public class Mail : ValueObject
    {
        public string Value { get; }

        public const string RegexPattern = @"[^@ \t\r\n]*@[^@ \t\r\n]+\.[^@ \t\r\n]+";
        static Regex Regex = new(RegexPattern);

        public Mail(string value)
        {
            Value = value;
        }

        public static Result<Mail> Create(string mail)
        {
            if (!mail.Contains('@'))
            {
                return Result.Failure<Mail>(new Error(
                    "Mail.No@",
                    "The mail must consist of exactly one @"
                    ));
            }
            if (!Regex.IsMatch(mail))
            {
                return Result.Failure<Mail>(new Error(
                    "Mail.InvalidFormat",
                    "The mail must have at least two texts separated by a ., after the @ symbol"
                    ));
            }
            return new Mail(mail);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
