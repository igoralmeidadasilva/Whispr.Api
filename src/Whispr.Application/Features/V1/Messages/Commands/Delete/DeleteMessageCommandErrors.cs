namespace Whispr.Application.Features.V1.Messages.Commands.Delete;

public static class DeleteMessageCommandErrors
{
    public static Error MessageIdIsRequired => Error.Create(
        "DeleteMessageCommand.MessageId.IsRequired",
        "Message ID is required.",
        ErrorType.Validation);

    public static Error MessageNotFound => Error.Create(
        "DeleteMessageCommand.Message.NotFound",
        "Message not found.",
        ErrorType.NotFound);
}