using Whispr.SharedKernel.Results;

namespace Whispr.Application.Features.V1.Users.Commands.Create;

public static class CreateUserCommandValidationErrors
{
    public static string UserNameIsRequired = "User name is required.";
    public static string UserNameMinLength = $"The user name must be at least {Domain.Constants.Constraints.User.UserNameMinLength} characters long.";
    public static string UserNameMaxLength = $"The user name must be a maximum of {Domain.Constants.Constraints.User.UserNameMaxLength} characters long.";
    public static string UserNameAlreadyExists = "User name already exists.";

    public static string EmailIsRequired = "Email is required.";
    public static string EmailFormat = "Email format is invalid.";
    public static string EmailAlreadyExists = "Email already exists.";

    public static string PasswordIsRequired = "Password is required.";
    public static string PasswordMinLength = $"The password must be at least {Domain.Constants.Constraints.User.PasswordMinLength} characters long.";
    public static string PasswordMaxLength = $"The password must be a maximum of {Domain.Constants.Constraints.User.PasswordMaxLength} characters long.";
    public static string PasswordFormatInvalidUpperCase = "User password must contain at least one uppercase letter.";
    public static string PasswordFormatInvalidLowerCase = "User password must contain at least one lowercase letter.";
    public static string PasswordFormatInvalidNumber = "User password must contain at least one number.";
    public static string PasswordFormatNonAlphanumeric = "User password must contain at least one special character.";
}