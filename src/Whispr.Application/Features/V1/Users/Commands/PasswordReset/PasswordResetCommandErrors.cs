using Whispr.SharedKernel.Results.Errors;

namespace Whispr.Application.Features.V1.Users.Commands.PasswordReset;

public static class PasswordResetCommandErrors
{
    public static Error EmailIsRequired => Error.Create(
        "PasswordResetCommand.Email.IsRequired",
        "Email is required.",
        ErrorType.Validation);

    public static Error EmailFormat => Error.Create(
        "PasswordResetCommand.Email.Format",
        "Email format is invalid.",
        ErrorType.Validation);

    public static Error EmailNotFound => Error.Create(
        "PasswordResetCommand.Email.NotFound",
        "Email not found.",
        ErrorType.NotFound);

    public static Error RecoveryCodeIsRequired => Error.Create(
        "PasswordResetCommand.RecoveryCode.IsRequired",
        "Recovery code is required.",
        ErrorType.Validation);

    public static Error RecoveryCodeInvalid => Error.Create(
        "PasswordResetCommand.RecoveryCode.Invalid",
        "Recovery code is invalid.",
        ErrorType.Validation);

    public static Error RecoveryCodeNotMatch => Error.Create(
        "PasswordResetCommand.RecoveryCode.NotMatch",
        "Recovery code does not match.",
        ErrorType.BadRequest);

    public static Error PasswordIsRequired => Error.Create(
        "PasswordResetCommand.Password.IsRequired",
        "Password is required.",
        ErrorType.Validation);

    public static Error PasswordMinLength => Error.Create(
        "PasswordResetCommand.Password.MinLength",
        $"The password must be at least {Domain.Constants.Constraints.User.PasswordMinLength} characters long.",
        ErrorType.Validation);

    public static Error PasswordMaxLength => Error.Create(
        "PasswordResetCommand.Password.MaxLength",
        $"The password must be a maximum of {Domain.Constants.Constraints.User.PasswordMaxLength} characters long.",
        ErrorType.Validation);

    public static Error PasswordFormatInvalidUpperCase => Error.Create(
        "PasswordResetCommand.Password.UpperCase",
        "User password must contain at least one uppercase letter.",
        ErrorType.Validation);

    public static Error PasswordFormatInvalidLowerCase => Error.Create(
        "PasswordResetCommand.Password.LowerCase",
        "User password must contain at least one lowercase letter.",
        ErrorType.Validation);

    public static Error PasswordFormatInvalidNumber => Error.Create(
        "PasswordResetCommand.Password.Number",
        "User password must contain at least one number.",
        ErrorType.Validation);

    public static Error PasswordFormatNonAlphanumeric => Error.Create(
        "PasswordResetCommand.Password.Alphanumeric",
        "User password must contain at least one special character.",
        ErrorType.Validation);

    public static Error ConfirmPasswordIsRequired => Error.Create(
        "PasswordResetCommand.ConfirmPassword.IsRequired",
        "Password is required.",
        ErrorType.Validation);

    public static Error ConfirmPasswordMinLength => Error.Create(
        "PasswordResetCommand.ConfirmPassword.MinLength",
        $"The confirm password must be at least {Domain.Constants.Constraints.User.PasswordMinLength} characters long.",
        ErrorType.Validation);

    public static Error ConfirmPasswordMaxLength => Error.Create(
        "PasswordResetCommand.ConfirmPassword.MaxLength",
        $"The confirm password must be a maximum of {Domain.Constants.Constraints.User.PasswordMaxLength} characters long.",
        ErrorType.Validation);

    public static Error ConfirmPasswordFormatInvalidUpperCase => Error.Create(
        "PasswordResetCommand.ConfirmPassword.UpperCase",
        "User confirm password must contain at least one uppercase letter.",
        ErrorType.Validation);

    public static Error ConfirmPasswordFormatInvalidLowerCase => Error.Create(
        "PasswordResetCommand.ConfirmPassword.LowerCase",
        "User confirm password must contain at least one lowercase letter.",
        ErrorType.Validation);

    public static Error ConfirmPasswordFormatInvalidNumber => Error.Create(
        "PasswordResetCommand.ConfirmPassword.Number",
        "User confirm password must contain at least one number.",
        ErrorType.Validation);

    public static Error ConfirmPasswordFormatNonAlphanumeric => Error.Create(
        "PasswordResetCommand.ConfirmPassword.Alphanumeric",
        "User confirm password must contain at least one special character.",
        ErrorType.Validation);

    public static Error ConfirmPasswordNotEquals => Error.Create(
        "PasswordResetCommand.ConfirmPassword.NotEquals",
        "User confirm password is not equals than password.",
        ErrorType.Validation);

    public static Error PasswordResetTokenNotFound => Error.Create(
        "PasswordResetCommand.PasswordResetToken.NotFound",
        "Password reset token not found.",
        ErrorType.NotFound);

    public static Error PasswordResetTokenInvalid => Error.Create(
        "PasswordResetCommand.PasswordResetToken.Invalid",
        "Password reset token is invalid.",
        ErrorType.Validation);
}