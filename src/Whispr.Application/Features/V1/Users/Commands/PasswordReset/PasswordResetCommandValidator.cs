namespace Whispr.Application.Features.V1.Users.Commands.PasswordReset;

public sealed class PasswordResetCommandValidator : AbstractValidator<PasswordResetCommand>
{
    public PasswordResetCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
                .WithError(PasswordResetCommandErrors.EmailIsRequired)
            .EmailAddress()
                .WithError(PasswordResetCommandErrors.EmailFormat)
                .When(x => !string.IsNullOrWhiteSpace(x.Email), ApplyConditionTo.CurrentValidator);

        RuleFor(x => x.RecoveryCode)
            .NotEmpty()
                .WithError(PasswordResetCommandErrors.RecoveryCodeIsRequired)
            .Length(Domain.Constants.Constraints.User.PasswordRecoveryCodeLength, Domain.Constants.Constraints.User.PasswordRecoveryCodeLength)
                .WithError(PasswordResetCommandErrors.RecoveryCodeInvalid);

        RuleFor(x => x.NewPassword)
            .NotEmpty()
                .WithError(PasswordResetCommandErrors.PasswordIsRequired)
            .MinimumLength(Domain.Constants.Constraints.User.PasswordMinLength)
                .WithError(PasswordResetCommandErrors.PasswordMinLength)
                .When(x => !string.IsNullOrWhiteSpace(x.NewPassword), ApplyConditionTo.CurrentValidator)
            .MaximumLength(Domain.Constants.Constraints.User.PasswordMaxLength)
                .WithError(PasswordResetCommandErrors.PasswordMaxLength)
                .When(x => !string.IsNullOrWhiteSpace(x.NewPassword), ApplyConditionTo.CurrentValidator)
            .Must(x => x.Any(char.IsUpper))
                .WithError(PasswordResetCommandErrors.PasswordFormatInvalidUpperCase)
                .When(x => !string.IsNullOrWhiteSpace(x.NewPassword), ApplyConditionTo.CurrentValidator)
            .Must(x => x.Any(char.IsLower))
                .WithError(PasswordResetCommandErrors.PasswordFormatInvalidLowerCase)
                .When(x => !string.IsNullOrWhiteSpace(x.NewPassword), ApplyConditionTo.CurrentValidator)
            .Must(x => x.Any(char.IsDigit))
                .WithError(PasswordResetCommandErrors.PasswordFormatInvalidNumber)
                .When(x => !string.IsNullOrWhiteSpace(x.NewPassword), ApplyConditionTo.CurrentValidator)
            .Matches(Domain.Constants.Constraints.User.PasswordFormat)
                .WithError(PasswordResetCommandErrors.PasswordFormatNonAlphanumeric)
                .When(x => !string.IsNullOrWhiteSpace(x.NewPassword), ApplyConditionTo.CurrentValidator);

        RuleFor(x => x.ConfirmNewPassword)
            .NotEmpty()
                .WithError(PasswordResetCommandErrors.ConfirmPasswordIsRequired)
            .MinimumLength(Domain.Constants.Constraints.User.PasswordMinLength)
                .WithError(PasswordResetCommandErrors.ConfirmPasswordMinLength)
                .When(x => !string.IsNullOrWhiteSpace(x.ConfirmNewPassword), ApplyConditionTo.CurrentValidator)
            .MaximumLength(Domain.Constants.Constraints.User.PasswordMaxLength)
                .WithError(PasswordResetCommandErrors.ConfirmPasswordMaxLength)
                .When(x => !string.IsNullOrWhiteSpace(x.ConfirmNewPassword), ApplyConditionTo.CurrentValidator)
            .Must(x => x.Any(char.IsUpper))
                .WithError(PasswordResetCommandErrors.ConfirmPasswordFormatInvalidUpperCase)
                .When(x => !string.IsNullOrWhiteSpace(x.ConfirmNewPassword), ApplyConditionTo.CurrentValidator)
            .Must(x => x.Any(char.IsLower))
                .WithError(PasswordResetCommandErrors.ConfirmPasswordFormatInvalidLowerCase)
                .When(x => !string.IsNullOrWhiteSpace(x.ConfirmNewPassword), ApplyConditionTo.CurrentValidator)
            .Must(x => x.Any(char.IsDigit))
                .WithError(PasswordResetCommandErrors.ConfirmPasswordFormatInvalidNumber)
                .When(x => !string.IsNullOrWhiteSpace(x.ConfirmNewPassword), ApplyConditionTo.CurrentValidator)
            .Matches(Domain.Constants.Constraints.User.PasswordFormat)
                .WithError(PasswordResetCommandErrors.ConfirmPasswordFormatNonAlphanumeric)
                .When(x => !string.IsNullOrWhiteSpace(x.ConfirmNewPassword), ApplyConditionTo.CurrentValidator)
            .Equal(x => x.NewPassword)
                .WithError(PasswordResetCommandErrors.ConfirmPasswordNotEquals)
                .When(x => !string.IsNullOrWhiteSpace(x.ConfirmNewPassword), ApplyConditionTo.CurrentValidator);
    }
}   