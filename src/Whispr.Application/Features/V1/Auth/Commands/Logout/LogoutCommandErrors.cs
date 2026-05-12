using Whispr.SharedKernel.Results.Errors;

namespace Whispr.Application.Features.V1.Auth.Commands.Logout;

public static class LogoutCommandErrors
{
    public static Error RefreshTokenIsRequired => Error.Create(
        "LogoutCommand.RefreshToken.IsRequired",
        "Refresh Token is required.",
        ErrorType.Validation);

    public static Error RefreshTokenNotFound => Error.Create(
        "LogoutCommand.RefreshToken.NotFound",
        "Refresh Token not found.",
        ErrorType.NotFound);
}