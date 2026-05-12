using Whispr.SharedKernel.Results.Errors;

namespace Whispr.Application.Features.V1.Auth.Commands.Login;

public static class LoginCommandErrors
{
    public static Error EmailIsRequired => Error.Create(
        "LoginCommand.Email.IsRequired",
        "Email is required.",
        ErrorType.Validation);

    public static Error EmailFormat => Error.Create(
        "LoginCommand.Email.Format",
        "Email format is invalid.",
        ErrorType.Validation);

    public static Error PasswordIsRequired => Error.Create(
        "LoginCommand.Password.IsRequired",
        "Password is required.",
        ErrorType.Validation);

    public static Error PasswordMinLength => Error.Create(
        "LoginCommand.Password.MinLength",
        $"The password must be at least {Domain.Constants.Constraints.User.PasswordMinLength} characters long.",
        ErrorType.Validation);

    public static Error PasswordMaxLength => Error.Create(
        "LoginCommand.Password.MaxLength",
        $"The password must be a maximum of {Domain.Constants.Constraints.User.PasswordMaxLength} characters long.",
        ErrorType.Validation);

    public static Error PasswordFormatInvalidUpperCase => Error.Create(
        "LoginCommand.Password.UpperCase",
        "User password must contain at least one uppercase letter.",
        ErrorType.Validation);

    public static Error PasswordFormatInvalidLowerCase => Error.Create(
        "LoginCommand.Password.LowerCase",
        "User password must contain at least one lowercase letter.",
        ErrorType.Validation);

    public static Error PasswordFormatInvalidNumber => Error.Create(
        "LoginCommand.Password.Number",
        "User password must contain at least one number.",
        ErrorType.Validation);

    public static Error PasswordFormatNonAlphanumeric => Error.Create(
        "LoginCommand.Password.Alphanumeric",
        "User password must contain at least one special character.",
        ErrorType.Validation);

    public static Error EmailNotFound => Error.Create(
        "LoginCommand.Email.NotFound",
        "Email not found.",
        ErrorType.NotFound);

    public static Error InvalidPassword => Error.Create(
        "LoginCommand.Password.Invalid",
        "Invalid password.",
        ErrorType.Unauthorized);
}