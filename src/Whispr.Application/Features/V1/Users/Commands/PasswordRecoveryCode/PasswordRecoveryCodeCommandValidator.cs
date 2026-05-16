namespace Whispr.Application.Features.V1.Users.Commands.PasswordRecoveryCode;

public sealed class PasswordRecoveryCodeCommandValidator : AbstractValidator<PasswordRecoveryCodeCommand>
{
    public PasswordRecoveryCodeCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
                .WithError(PasswordRecoveryCodeCommandErrors.EmailIsRequired)
            .EmailAddress()
                .WithError(PasswordRecoveryCodeCommandErrors.EmailFormat)
                .When(x => !string.IsNullOrWhiteSpace(x.Email), ApplyConditionTo.CurrentValidator);
    }
}