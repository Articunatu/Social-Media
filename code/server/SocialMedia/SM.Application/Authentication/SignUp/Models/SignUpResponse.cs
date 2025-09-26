namespace SM.Application.Authentication.SignUp.Models;

public record SignUpResponse(
    Guid Id,
    string Tag,
    string FullName,
    string Email
);
