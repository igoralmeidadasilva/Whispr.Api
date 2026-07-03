namespace Whispr.Application.Features.V1.Messages.Commands.Delete;

public sealed class DeleteMessageCommandValidator : AbstractValidator<DeleteMessageCommand>
{
    public DeleteMessageCommandValidator()
    {
        RuleFor(x => x.MessageId)
            .NotEqual(Guid.Empty)
                .WithError(DeleteMessageCommandErrors.MessageIdIsRequired);
    }
}