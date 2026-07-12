namespace Whispr.Application.Features.V1.Messages.Commands.Update;

public sealed class UpdateMessageCommandValidator : AbstractValidator<UpdateMessageCommand>
{
    public UpdateMessageCommandValidator()
    {
        RuleFor(x => x.MessageId)
            .NotEqual(Guid.Empty)
                .WithError(UpdateMessageCommandErrors.MessageIdIsRequired);

        RuleFor(x => x.Content)
            .NotEmpty()
                .WithError(UpdateMessageCommandErrors.ContentIsRequired);
    }
}