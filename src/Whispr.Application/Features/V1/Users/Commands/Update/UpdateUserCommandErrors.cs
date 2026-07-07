namespace Whispr.Application.Features.V1.Users.Commands.Update;

public static class UpdateUserCommandErrors
{
    public static Error UserIdNotFound => Error.Create(
        "UpdateUserCommand.UserId.NotFound",
        "User id cannot be found.",
        ErrorType.NotFound);

    public static Error UserNameIsRequired => Error.Create(
        "UpdateUserCommand.UserName.IsRequired",
        "User name is required.",
        ErrorType.Validation);

    public static Error UserNameMinLength => Error.Create(
        "UpdateUserCommand.UserName.MinLength",
        $"The user name must be at least {Domain.Constants.Constraints.User.NameMinLength} characters long.",
        ErrorType.Validation);

    public static Error UserNameMaxLength => Error.Create(
        "UpdateUserCommand.UserName.MaxLength",
        $"The user name must be a maximum of {Domain.Constants.Constraints.User.NameMaxLength} characters long.",
        ErrorType.Validation);

    public static Error UserNameAlreadyExists => Error.Create(
        "UpdateUserCommand.UserName.AlreadyExists",
        "User name already exists.",
        ErrorType.Conflict);

    public static Error EmailIsRequired => Error.Create(
        "UpdateUserCommand.Email.IsRequired",
        "Email is required.",
        ErrorType.Validation);

    public static Error EmailFormat => Error.Create(
        "UpdateUserCommand.Email.Format",
        "Email format is invalid.",
        ErrorType.Validation);

    public static Error EmailAlreadyExists => Error.Create(
        "UpdateUserCommand.Email.AlreadyExists",
        "Email already exists.",
        ErrorType.Conflict);
}