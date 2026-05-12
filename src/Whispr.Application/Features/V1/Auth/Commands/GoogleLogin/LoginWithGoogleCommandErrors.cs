using Whispr.SharedKernel.Results.Errors;

namespace Whispr.Application.Features.V1.Auth.Commands.GoogleLogin;

public static class LoginWithGoogleCommandErrors
{
    public static Error EmailNotFound => Error.Create(
        "LoginWithGoogleCommand.Email.NotFound",
        "Email not found.",
        ErrorType.BadRequest);

    public static Error NameNotFound => Error.Create(
        "LoginWithGoogleCommand.Name.NotFound",
        "Name not found.",
        ErrorType.BadRequest);

    public static Error IdTokenIsRequired => Error.Create(
        "LoginWithGoogleCommand.IdToken.IsRequired",
        "Id Token is required.",
        ErrorType.Validation);
}