namespace Whispr.Application.Features.V1.Users.Commands.Create;

public static class CreateUserCommandErrors
{
    public static Error NameIsRequired => Error.Create(
        "CreateUserCommand.Name.IsRequired",
        "User name is required.",
        ErrorType.Validation);

    public static Error NameMinLength => Error.Create(
        "CreateUserCommand.Name.MinLength",
        $"The user name must be at least {Domain.Constants.Constraints.User.NameMinLength} characters long.",
        ErrorType.Validation);

    public static Error NameMaxLength => Error.Create(
        "CreateUserCommand.Name.MaxLength",
        $"The user name must be a maximum of {Domain.Constants.Constraints.User.NameMaxLength} characters long.",
        ErrorType.Validation);

    public static Error NameAlreadyExists => Error.Create(
        "CreateUserCommand.Name.AlreadyExists",
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

    // ---------
    public static Error ConfirmPasswordIsRequired => Error.Create(
        "CreateUserCommand.ConfirmPassword.IsRequired",
        "Password is required.",
        ErrorType.Validation);

    public static Error ConfirmPasswordMinLength => Error.Create(
        "CreateUserCommand.ConfirmPassword.MinLength",
        $"The confirm password must be at least {Domain.Constants.Constraints.User.PasswordMinLength} characters long.",
        ErrorType.Validation);

    public static Error ConfirmPasswordMaxLength => Error.Create(
        "CreateUserCommand.ConfirmPassword.MaxLength",
        $"The confirm password must be a maximum of {Domain.Constants.Constraints.User.PasswordMaxLength} characters long.",
        ErrorType.Validation);

    public static Error ConfirmPasswordFormatInvalidUpperCase => Error.Create(
        "CreateUserCommand.ConfirmPassword.UpperCase",
        "User confirm password must contain at least one uppercase letter.",
        ErrorType.Validation);

    public static Error ConfirmPasswordFormatInvalidLowerCase => Error.Create(
        "CreateUserCommand.ConfirmPassword.LowerCase",
        "User confirm password must contain at least one lowercase letter.",
        ErrorType.Validation);

    public static Error ConfirmPasswordFormatInvalidNumber => Error.Create(
        "CreateUserCommand.ConfirmPassword.Number",
        "User confirm password must contain at least one number.",
        ErrorType.Validation);

    public static Error ConfirmPasswordFormatNonAlphanumeric => Error.Create(
        "CreateUserCommand.ConfirmPassword.Alphanumeric",
        "User confirm password must contain at least one special character.",
        ErrorType.Validation);

    public static Error ConfirmPasswordNotEquals => Error.Create(
        "CreateUserCommand.ConfirmPassword.NotEquals",
        "User confirm password is not equals than password.",
        ErrorType.Validation);
}