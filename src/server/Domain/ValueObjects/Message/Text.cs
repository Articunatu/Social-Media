using Domain.Primitives;

namespace Domain.ValueObjects.Message
{
    public class Text : ValueObject
    {
        public string Value { get; }
        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
