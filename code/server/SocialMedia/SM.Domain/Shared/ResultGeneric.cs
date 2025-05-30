namespace SM.Domain.Shared;

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        if (isSuccess && value == null)
            throw new ArgumentNullException(nameof(value), "Successful result must have a non-null value");

        _value = value;
    }

    protected internal Result(bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        if (isSuccess)
            throw new InvalidOperationException("Successful result must have a value");

        _value = default;
    }

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result cannot be accessed");

    public static implicit operator Result<TValue>(TValue value) => Create(value);

    public static Result<TValue> Create(TValue value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value), "The value cannot be null");

        return new Result<TValue>(value, true, Error.None);
    }
}
