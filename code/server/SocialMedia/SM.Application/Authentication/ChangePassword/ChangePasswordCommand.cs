using SM.Application.Abstractions;

namespace SM.Application.Authentication.ChangePassword;

public record ChangePasswordCommand(Guid UserId, string OldPassword, string NewPassword, string ConfirmPassword) 
    :ICommand<string>;