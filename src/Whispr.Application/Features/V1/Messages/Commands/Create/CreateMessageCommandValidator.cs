namespace Whispr.Application.Features.V1.Messages.Commands.Create;

public sealed class CreateMessageCommandValidator : AbstractValidator<CreateMessageCommand>
{
    public CreateMessageCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty)
                .WithError(CreateMessageCommandErrors.UserIdIsRequired);

        RuleFor(x => x.Content)
            .NotEmpty()
                .WithError(CreateMessageCommandErrors.ContentOrAttachmentRequired)
            .When(x => x.Attachments == null || !x.Attachments.Any());
    }
}