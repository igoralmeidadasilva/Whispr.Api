using Whispr.SharedKernel.Results.Errors;

namespace Whispr.Application.Features.V1.Users.Commands.CreatePasswordRecoveryCode;

internal static class CreatePasswordRecoveryCodeCommandErrors
{
    public static Error EmailIsRequired => Error.Create(
       "CreatePasswordRecoveryCodeCommand.Email.IsRequired",
       "Email is required.",
       ErrorType.Validation);

    public static Error EmailFormat => Error.Create(
        "CreatePasswordRecoveryCodeCommand.Email.Format",
        "Email format is invalid.",
        ErrorType.Validation);

    public static Error EmailNotFound => Error.Create(
        "CreatePasswordRecoveryCodeCommand.Email.NotFound",
        "Email not found.",
        ErrorType.NotFound);
}