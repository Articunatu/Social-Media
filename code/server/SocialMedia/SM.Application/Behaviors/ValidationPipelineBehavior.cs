using FluentValidation;
using MediatR;
using SM.Domain.Shared;

namespace SM.Application.Behaviors;

public class ValidationPipelineBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next(cancellationToken);
        }

        var validationResults = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(request, cancellationToken)));

        var errors = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .Select(f => new Error(f.PropertyName, f.ErrorMessage))
            .Distinct()
            .ToArray();

        if (errors.Length > 0)
        {
            return CreateValidationResult<TResponse>(errors);
        }

        return await next(cancellationToken);
    }

    private static TResponse CreateValidationResult<TResponse>(Error[] errors)
        where TResponse : Result
    {
        if (typeof(TResponse) == typeof(Result))
        {
            return (ValidationResult.WithErrors(errors) as TResponse)!;
        }

        var genericType = typeof(TResponse).GetGenericArguments().First();
        var method = typeof(ValidationResult<>)
            .MakeGenericType(genericType)
            .GetMethod(nameof(ValidationResult<object>.WithErrors))!;

        var validationResult = method.Invoke(null, [errors])!;

        return (TResponse)validationResult;
    }
}
