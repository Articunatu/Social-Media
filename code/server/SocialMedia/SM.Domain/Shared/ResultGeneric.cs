using System.Net;

namespace SM.Domain.Shared;

public class Result<T> : Result
{
    private readonly T? _value;

    protected internal Result(T? value, bool isSuccess, Error error, HttpStatusCode status)
        : base(isSuccess, error, status)
    {
        _value = value;
    }

    protected internal Result(bool isSuccess, Error error, HttpStatusCode status)
        : base(isSuccess, error, status)
    {
        _value = default;
    }

    public T? Value => _value;

    public bool TryGetValue(out T? value)
    {
        value = _value;
        return IsSuccess;
    }

    public static implicit operator Result<T>(T? value) => Create(value);

    public static Result<T> Create(T? value)
        => new(value, true, Error.None, HttpStatusCode.OK);
}
