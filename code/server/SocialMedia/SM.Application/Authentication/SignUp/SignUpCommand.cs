using MediatR;
using SM.Domain.Shared;

namespace SM.Application.Authentication.SignUp;

public record SignUpCommand(string Tag, string Password) : IRequest<Result>;