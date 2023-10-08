using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Abstractions
{
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        protected readonly ISender _sender;

        protected ApiController(ISender sender) 
        {
            _sender = sender;
        }

        protected IActionResult HandleFailure(Result result)
        {
            return result switch
            {
                { IsSuccess: true } => throw new InvalidOperationException(),
                IValidationResult validationResult =>
                    BadRequest(
                        CreateProblemDetails(
                            "Validation Error", StatusCodes.Status400BadRequest,
                            result.Error,
                            validationResult.Errors)),
                _ =>
                    BadRequest(
                        CreateProblemDetails(
                            "Bad Request",
                            StatusCodes.Status400BadRequest,
                            result.Error))
            };
        }



        static ProblemDetails CreateProblemDetails(
            string title,
            int status,
            Error error,
            Error[]? errors = null) =>
            new()
            {
                Title = title,
                Type = error.Header,
                Detail = error.Message,
                Status = status,
                Extensions = { { nameof(errors), errors } },
            };
    }
}
