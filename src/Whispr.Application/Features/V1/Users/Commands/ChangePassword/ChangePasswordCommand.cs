namespace Whispr.Application.Features.V1.Users.Commands.ChangePassword;

public sealed record ChangePasswordCommand : ICommand<Unit>
{
    public required string Email { get; init; }
    public required string RecoveryCode { get; init; }
    public required string NewPassword { get; init; }
    public required string ConfirmNewPassword { get; init; }
}