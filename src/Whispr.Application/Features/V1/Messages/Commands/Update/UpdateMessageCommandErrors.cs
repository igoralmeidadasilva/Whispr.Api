namespace Whispr.Application.Features.V1.Messages.Commands.Update;

public static class UpdateMessageCommandErrors
{
    public static Error MessageIdIsRequired => Error.Create(
        "UpdateMessageCommand.MessageId.IsRequired",
        "Message ID is required.",
        ErrorType.Validation);

    public static Error ContentIsRequired => Error.Create(
        "UpdateMessageCommand.Content.IsRequired",
        "Message content is required.",
        ErrorType.Validation);

    public static Error MessageNotFound => Error.Create(
        "UpdateMessageCommand.Message.NotFound",
        "Message not found.",
        ErrorType.NotFound);
}