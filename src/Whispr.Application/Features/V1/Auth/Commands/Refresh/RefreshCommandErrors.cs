namespace Whispr.Application.Features.V1.Auth.Commands.Refresh;

public static class RefreshCommandErrors
{
    public static Error InvalidAccessToken => Error.Create(
        "RefreshCommand.ExpiredAccessToken.Invalid",
        "Invalid access token.",
        ErrorType.BadRequest);

    public static Error ExpiredRefreshToken => Error.Create(
        "RefreshCommand.ExpiredRefreshToken.Invalid",
        "Invalid refresh token.",
        ErrorType.BadRequest);

    public static Error InvalidRefreshToken => Error.Create(
        "RefreshCommand.InvalidRefreshToken.Invalid",
        "Invalid refresh token.",
        ErrorType.BadRequest);

    public static Error RefreshTokenNotFound => Error.Create(
        "RefreshCommand.RefreshTokenNotFound",
        "Refresh token not found.",
        ErrorType.NotFound);
}