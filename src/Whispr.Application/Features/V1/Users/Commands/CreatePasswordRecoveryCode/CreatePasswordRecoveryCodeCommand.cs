namespace Whispr.Application.Features.V1.Users.Commands.CreatePasswordRecoveryCode;

public sealed record CreatePasswordRecoveryCodeCommand : ICommand<Unit>
{
    public required string Email { get; init; }
}