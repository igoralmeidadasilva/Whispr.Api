namespace Whispr.Application.Features.V1.Messages.Commands.Create;

public static class CreateMessageCommandErrors
{
public static Error UserIdIsRequired => Error.Create(
        "CreateMessageCommand.UserId.IsRequired",
        "User ID is required.",
        ErrorType.Validation);

    public static Error ContentOrAttachmentRequired => Error.Create(
        "CreateMessageCommand.ContentOrAttachment.Required",
        "A message must contain either text content or at least one attachment.",
        ErrorType.Validation);

    public static Error UserNotFound => Error.Create(
        "CreateMessageCommand.User.NotFound",
        "User not found.",
        ErrorType.NotFound);
}