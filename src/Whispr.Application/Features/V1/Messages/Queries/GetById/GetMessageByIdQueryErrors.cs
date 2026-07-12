namespace Whispr.Application.Features.V1.Messages.Queries.GetById;

public static class GetMessageByIdQueryErrors
{
    public static Error MessageIdIsRequired => Error.Create(
        "GetMessageByIdQuery.MessageId.IsRequired",
        "MessageId is required.",
        ErrorType.Validation);
    
    public static Error MessageNotFound => Error.Create(
        "GetMessageByIdQuery.MessageNotFound",
        "No message was found with this Id: ",
        ErrorType.NotFound);
}