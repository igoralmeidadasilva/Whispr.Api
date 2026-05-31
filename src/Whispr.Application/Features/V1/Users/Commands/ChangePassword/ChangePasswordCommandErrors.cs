using Whispr.SharedKernel.Results.Errors;

namespace Whispr.Application.Features.V1.Users.Commands.ChangePassword;

public static class ChangePasswordCommandErrors
{
    public static Error EmailIsRequired => Error.Create(
        "ChangePasswordCommand.Email.IsRequired",
        "Email is required.",
        ErrorType.Validation);

    public static Error EmailFormat => Error.Create(
        "ChangePasswordCommand.Email.Format",
        "Email format is invalid.",
        ErrorType.Validation);

    public static Error EmailNotFound => Error.Create(
        "ChangePasswordCommand.Email.NotFound",
        "Email not found.",
        ErrorType.NotFound);

    public static Error RecoveryCodeIsRequired => Error.Create(
        "ChangePasswordCommand.RecoveryCode.IsRequired",
        "Recovery code is required.",
        ErrorType.Validation);

    public static Error RecoveryCodeInvalid => Error.Create(
        "ChangePasswordCommand.RecoveryCode.Invalid",
        "Recovery code is invalid.",
        ErrorType.Validation);

    public static Error RecoveryCodeNotMatch => Error.Create(
        "ChangePasswordCommand.RecoveryCode.NotMatch",
        "Recovery code does not match.",
        ErrorType.BadRequest);

    public static Error PasswordIsRequired => Error.Create(
        "ChangePasswordCommand.Password.IsRequired",
        "Password is required.",
        ErrorType.Validation);

    public static Error PasswordMinLength => Error.Create(
        "ChangePasswordCommand.Password.MinLength",
        $"The password must be at least {Domain.Constants.Constraints.User.PasswordMinLength} characters long.",
        ErrorType.Validation);

    public static Error PasswordMaxLength => Error.Create(
        "ChangePasswordCommand.Password.MaxLength",
        $"The password must be a maximum of {Domain.Constants.Constraints.User.PasswordMaxLength} characters long.",
        ErrorType.Validation);

    public static Error PasswordFormatInvalidUpperCase => Error.Create(
        "ChangePasswordCommand.Password.UpperCase",
        "User password must contain at least one uppercase letter.",
        ErrorType.Validation);

    public static Error PasswordFormatInvalidLowerCase => Error.Create(
        "ChangePasswordCommand.Password.LowerCase",
        "User password must contain at least one lowercase letter.",
        ErrorType.Validation);

    public static Error PasswordFormatInvalidNumber => Error.Create(
        "ChangePasswordCommand.Password.Number",
        "User password must contain at least one number.",
        ErrorType.Validation);

    public static Error PasswordFormatNonAlphanumeric => Error.Create(
        "ChangePasswordCommand.Password.Alphanumeric",
        "User password must contain at least one special character.",
        ErrorType.Validation);

    public static Error ConfirmPasswordIsRequired => Error.Create(
        "ChangePasswordCommand.ConfirmPassword.IsRequired",
        "Password is required.",
        ErrorType.Validation);

    public static Error ConfirmPasswordMinLength => Error.Create(
        "ChangePasswordCommand.ConfirmPassword.MinLength",
        $"The confirm password must be at least {Domain.Constants.Constraints.User.PasswordMinLength} characters long.",
        ErrorType.Validation);

    public static Error ConfirmPasswordMaxLength => Error.Create(
        "ChangePasswordCommand.ConfirmPassword.MaxLength",
        $"The confirm password must be a maximum of {Domain.Constants.Constraints.User.PasswordMaxLength} characters long.",
        ErrorType.Validation);

    public static Error ConfirmPasswordFormatInvalidUpperCase => Error.Create(
        "ChangePasswordCommand.ConfirmPassword.UpperCase",
        "User confirm password must contain at least one uppercase letter.",
        ErrorType.Validation);

    public static Error ConfirmPasswordFormatInvalidLowerCase => Error.Create(
        "ChangePasswordCommand.ConfirmPassword.LowerCase",
        "User confirm password must contain at least one lowercase letter.",
        ErrorType.Validation);

    public static Error ConfirmPasswordFormatInvalidNumber => Error.Create(
        "ChangePasswordCommand.ConfirmPassword.Number",
        "User confirm password must contain at least one number.",
        ErrorType.Validation);

    public static Error ConfirmPasswordFormatNonAlphanumeric => Error.Create(
        "ChangePasswordCommand.ConfirmPassword.Alphanumeric",
        "User confirm password must contain at least one special character.",
        ErrorType.Validation);

    public static Error ConfirmPasswordNotEquals => Error.Create(
        "ChangePasswordCommand.ConfirmPassword.NotEquals",
        "User confirm password is not equals than password.",
        ErrorType.Validation);

    public static Error PasswordResetTokenNotFound => Error.Create(
        "ChangePasswordCommand.PasswordResetToken.NotFound",
        "Password reset token not found.",
        ErrorType.NotFound);

    public static Error PasswordResetTokenInvalid => Error.Create(
        "ChangePasswordCommand.PasswordResetToken.Invalid",
        "Password reset token is invalid.",
        ErrorType.Validation);
}