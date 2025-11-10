using Whispr.SharedKernel.Results;

namespace Whispr.Application.Features.V1.Users.Commands.Create;

public static class CreateUserCommandErrors
{
    public static Error UserNameIsRequired => Error.Create(
        "CreateUserCommand.UserName.IsRequired",
        "User name is required.",
        ErrorType.Validation);

    public static Error UserNameMinLength => Error.Create(
        "CreateUserCommand.UserName.MinLength",
        $"The user name must be at least {Domain.Constants.Constraints.User.UserNameMinLength} characters long.",
        ErrorType.Validation);

    public static Error UserNameMaxLength => Error.Create(
        "CreateUserCommand.UserName.MaxLength",
        $"The user name must be a maximum of {Domain.Constants.Constraints.User.UserNameMaxLength} characters long.",
        ErrorType.Validation);

    public static Error UserNameAlreadyExists => Error.Create(
        "CreateUserCommand.UserName.AlreadyExists",
        "User name already exists.",
        ErrorType.Conflict);

    public static Error EmailIsRequired => Error.Create(
        "CreateUserCommand.Email.IsRequired",
        "Email is required.",
        ErrorType.Validation);

    public static Error EmailFormat => Error.Create(
        "CreateUserCommand.Email.Format",
        "Email format is invalid.",
        ErrorType.Validation);

    public static Error EmailAlreadyExists => Error.Create(
        "CreateUserCommand.Email.AlreadyExists",
        "Email already exists.",
        ErrorType.Conflict);

    public static Error PasswordIsRequired => Error.Create(
        "CreateUserCommand.Password.IsRequired",
        "Password is required.",
        ErrorType.Validation);

    public static Error PasswordMinLength => Error.Create(
        "CreateUserCommand.Password.MinLength",
        $"The password must be at least {Domain.Constants.Constraints.User.PasswordMinLength} characters long.",
        ErrorType.Validation);

    public static Error PasswordMaxLength => Error.Create(
        "CreateUserCommand.Password.MaxLength",
        $"The password must be a maximum of {Domain.Constants.Constraints.User.PasswordMaxLength} characters long.",
        ErrorType.Validation);

    public static Error PasswordFormatInvalidUpperCase => Error.Create(
        "CreateUserCommand.Password.UpperCase",
        "User password must contain at least one uppercase letter.",
        ErrorType.Validation);

    public static Error PasswordFormatInvalidLowerCase => Error.Create(
        "CreateUserCommand.Password.LowerCase",
        "User password must contain at least one lowercase letter.",
        ErrorType.Validation);

    public static Error PasswordFormatInvalidNumber => Error.Create(
        "CreateUserCommand.Password.Number",
        "User password must contain at least one number.",
        ErrorType.Validation);

    public static Error PasswordFormatNonAlphanumeric => Error.Create(
        "CreateUserCommand.Password.Alphanumeric",
        "User password must contain at least one special character.",
        ErrorType.Validation);

    public static Error IdentityFailure(string message) => Error.Create(
        "CreateUserCommand.Identity.Failure",
        message,
        ErrorType.Failure);
}