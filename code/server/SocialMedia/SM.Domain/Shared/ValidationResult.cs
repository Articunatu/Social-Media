using System.Net;

namespace SM.Domain.Shared;

public sealed class ValidationResult : Result, IValidationResult
{
    private ValidationResult(Error[] errors)
        : base(false, IValidationResult.ValidationError, HttpStatusCode.BadRequest)
    {
        Errors = errors;
    }

    public Error[] Errors { get; }

    public static ValidationResult WithErrors(Error[] errors) => new(errors);
}
