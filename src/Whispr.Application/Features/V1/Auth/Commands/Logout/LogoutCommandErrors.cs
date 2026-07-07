namespace Whispr.Application.Features.V1.Auth.Commands.Logout;

public static class LogoutCommandErrors
{
    public static Error RefreshTokenIsRequired => Error.Create(
        "LogoutCommand.RefreshToken.IsRequired",
        "Refresh TokenHash is required.",
        ErrorType.Validation);

    public static Error RefreshTokenNotFound => Error.Create(
        "LogoutCommand.RefreshToken.NotFound",
        "Refresh TokenHash not found.",
        ErrorType.NotFound);
}