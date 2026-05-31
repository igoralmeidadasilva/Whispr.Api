namespace Whispr.Presentation.Web.Services.Api.V1.Users.Requests;

public sealed record ChangePasswordRequest
{
    public required string Email { get; init; }
    public required string RecoveryCode { get; init; }
    public required string NewPassword { get; init; }
    public required string ConfirmNewPassword { get; init; }
}