using System.Net;

namespace SM.Domain.Shared;

public sealed class ValidationResult<TValue> : Result<TValue>, IValidationResult
{
    private ValidationResult(Error[] errors)
        : base(default!, false, IValidationResult.ValidationError, HttpStatusCode.BadRequest)
    {
        Errors = errors;
    }

    public Error[] Errors { get; }

    public static ValidationResult<TValue> WithErrors(Error[] errors) => new(errors);
}
