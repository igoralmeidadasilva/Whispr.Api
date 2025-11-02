using Whispr.SharedKernel.Results;

namespace Whispr.Application.Commands.Users;

public static class CreateUserCommandValidationErrors
{
    public static Error UserNameIsRequired 
        => Error.Create("User.UserName.IsRequired", "User name is required", ErrorType.Validation);
    public static Error UserNameMinLength 
        => Error.Create("User.UserName.MinLength", $"The user name must be at least {Domain.Constants.Constraints.User.UserNameMinLength} characters long.", ErrorType.Validation);
    public static Error UserNameMaxLength 
        => Error.Create("User.UserName.MaxLength", $"The user name must be a maximum of {Domain.Constants.Constraints.User.UserNameMaxLength} characters long.", ErrorType.Validation);
    public static Error UserNameAlreadyExists
        => Error.Create("User.UserName.AlreadyExists", "User name already exists", ErrorType.Conflict);
    public static Error EmailIsRequired 
        => Error.Create("User.Email.IsRequired", "Email is required", ErrorType.Validation);
    public static Error EmailFormat
        => Error.Create("User.Email.Format", "Email format", ErrorType.Validation);
    public static Error EmailAlreadyExists
        => Error.Create("User.Email.AlreadyExists", "Email already exists", ErrorType.Conflict);
    public static Error PasswordIsRequired 
        => Error.Create("User.Password.IsRequired", "Password is required", ErrorType.Validation);
    public static Error PasswordMinLength 
        => Error.Create("User.Password.MinLength", $"The password must be at least {Domain.Constants.Constraints.User.UserNameMinLength} characters long.", ErrorType.Validation);
    public static Error PasswordMaxLength 
        => Error.Create("User.Password.MaxLength", $"The password must be a maximum of {Domain.Constants.Constraints.User.UserNameMaxLength} characters long.", ErrorType.Validation);
    public static Error PasswordFormatInvalidUpperCase
        => Error.Create("CreateUser.Password.RequiredUpperCase", "User password must contain at least one lowercase letter.", ErrorType.Validation);
    public static Error PasswordFormatInvalidLowerCase
        => Error.Create("CreateUser.Password.RequiredLowerCase", "User password must contain at least one lowercase letter.", ErrorType.Validation);
    public static Error PasswordFormatInvalidNumber
        => Error.Create("CreateUser.Password.RequiredNumber", "User password must contain at least one number;", ErrorType.Validation);
    public static Error PasswordFormatNonAlphanumeric
        => Error.Create("CreateUser.Password.RequiredNonAlphanumeric", "User password must contain at least one special character.", ErrorType.Validation);
    
}