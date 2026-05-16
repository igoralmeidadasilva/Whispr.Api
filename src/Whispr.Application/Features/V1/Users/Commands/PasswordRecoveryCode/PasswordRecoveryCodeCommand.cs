namespace Whispr.Application.Features.V1.Users.Commands.PasswordRecoveryCode;

public sealed record PasswordRecoveryCodeCommand : ICommand<Unit>
{
    public required string Email { get; init; }
}