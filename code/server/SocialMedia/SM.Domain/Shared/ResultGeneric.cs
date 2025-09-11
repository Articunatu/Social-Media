using System.Net;

namespace SM.Domain.Shared;

public class Result<T> : Result
{
    private readonly T? _value;

    protected internal Result(T value, bool isSuccess, Error error, HttpStatusCode status)
        : base(isSuccess, error, status)
    {
        if (isSuccess && value == null)
            throw new ArgumentNullException(nameof(value), "Successful result must have a non-null value.");

        _value = value;
    }

    protected internal Result(bool isSuccess, Error error, HttpStatusCode status)
        : base(isSuccess, error, status)
    {
        if (isSuccess)
            throw new InvalidOperationException("Successful result must have a value.");

        _value = default;
    }

    public T Value => IsSuccess ? _value! : throw new InvalidOperationException("No value present.");

    public static implicit operator Result<T>(T value) => Create(value);

    public static Result<T> Create(T value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value), "The value cannot be null.");

        return new Result<T>(value, true, Error.None, HttpStatusCode.OK);
    }
}
