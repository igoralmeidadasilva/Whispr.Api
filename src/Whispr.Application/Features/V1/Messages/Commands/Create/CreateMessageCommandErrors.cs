namespace Whispr.Application.Features.V1.Messages.Commands.Create;

public static class CreateMessageCommandErrors
{
    public static Error UserIdIsRequired => Error.Create(
        "CreateMessageCommand.UserId.IsRequired",
        "User ID is required.",
        ErrorType.Validation);

    public static Error ContentIsRequired => Error.Create(
        "CreateMessageCommand.Content.IsRequired",
        "Message content is required.",
        ErrorType.Validation);

    public static Error UserNotFound => Error.Create(
        "CreateMessageCommand.User.NotFound",
        "User not found.",
        ErrorType.NotFound);
}