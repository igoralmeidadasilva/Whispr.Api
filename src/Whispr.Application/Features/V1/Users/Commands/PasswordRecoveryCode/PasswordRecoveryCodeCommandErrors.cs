using Whispr.SharedKernel.Results.Errors;

namespace Whispr.Application.Features.V1.Users.Commands.PasswordRecoveryCode;

internal static class PasswordRecoveryCodeCommandErrors
{
    public static Error EmailIsRequired => Error.Create(
       "PasswordRecoveryCodeCommand.Email.IsRequired",
       "Email is required.",
       ErrorType.Validation);

    public static Error EmailFormat => Error.Create(
        "PasswordRecoveryCodeCommand.Email.Format",
        "Email format is invalid.",
        ErrorType.Validation);

    public static Error EmailNotFound => Error.Create(
        "PasswordRecoveryCodeCommand.Email.NotFound",
        "Email not found.",
        ErrorType.NotFound);
}