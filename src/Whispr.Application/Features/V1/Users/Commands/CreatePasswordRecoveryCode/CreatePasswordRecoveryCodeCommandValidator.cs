namespace Whispr.Application.Features.V1.Users.Commands.CreatePasswordRecoveryCode;

public sealed class CreatePasswordRecoveryCodeCommandValidator : AbstractValidator<CreatePasswordRecoveryCodeCommand>
{
    public CreatePasswordRecoveryCodeCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
                .WithError(CreatePasswordRecoveryCodeCommandErrors.EmailIsRequired)
            .EmailAddress()
                .WithError(CreatePasswordRecoveryCodeCommandErrors.EmailFormat)
                .When(x => !string.IsNullOrWhiteSpace(x.Email), ApplyConditionTo.CurrentValidator);
    }
}