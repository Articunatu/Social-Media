using SM.Application.Abstractions;

namespace SM.Application.Users.DeleteAccount;

public record DeleteAccountCommand(Guid Id) : ICommand<UserCommandResponse>;
