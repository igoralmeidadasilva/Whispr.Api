namespace Whispr.Application.Features.V1.Users.Commands.ChangePassword;

public sealed class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
                .WithError(ChangePasswordCommandErrors.EmailIsRequired)
            .EmailAddress()
                .WithError(ChangePasswordCommandErrors.EmailFormat)
                .When(x => !string.IsNullOrWhiteSpace(x.Email), ApplyConditionTo.CurrentValidator);

        RuleFor(x => x.RecoveryCode)
            .NotEmpty()
                .WithError(ChangePasswordCommandErrors.RecoveryCodeIsRequired)
            .Length(Domain.Constants.Constraints.User.PasswordRecoveryCodeLength, Domain.Constants.Constraints.User.PasswordRecoveryCodeLength)
                .WithError(ChangePasswordCommandErrors.RecoveryCodeInvalid);

        RuleFor(x => x.NewPassword)
            .NotEmpty()
                .WithError(ChangePasswordCommandErrors.PasswordIsRequired)
            .MinimumLength(Domain.Constants.Constraints.User.PasswordMinLength)
                .WithError(ChangePasswordCommandErrors.PasswordMinLength)
                .When(x => !string.IsNullOrWhiteSpace(x.NewPassword), ApplyConditionTo.CurrentValidator)
            .MaximumLength(Domain.Constants.Constraints.User.PasswordMaxLength)
                .WithError(ChangePasswordCommandErrors.PasswordMaxLength)
                .When(x => !string.IsNullOrWhiteSpace(x.NewPassword), ApplyConditionTo.CurrentValidator)
            .Must(x => x.Any(char.IsUpper))
                .WithError(ChangePasswordCommandErrors.PasswordFormatInvalidUpperCase)
                .When(x => !string.IsNullOrWhiteSpace(x.NewPassword), ApplyConditionTo.CurrentValidator)
            .Must(x => x.Any(char.IsLower))
                .WithError(ChangePasswordCommandErrors.PasswordFormatInvalidLowerCase)
                .When(x => !string.IsNullOrWhiteSpace(x.NewPassword), ApplyConditionTo.CurrentValidator)
            .Must(x => x.Any(char.IsDigit))
                .WithError(ChangePasswordCommandErrors.PasswordFormatInvalidNumber)
                .When(x => !string.IsNullOrWhiteSpace(x.NewPassword), ApplyConditionTo.CurrentValidator)
            .Matches(Domain.Constants.Constraints.User.PasswordFormat)
                .WithError(ChangePasswordCommandErrors.PasswordFormatNonAlphanumeric)
                .When(x => !string.IsNullOrWhiteSpace(x.NewPassword), ApplyConditionTo.CurrentValidator);

        RuleFor(x => x.ConfirmNewPassword)
            .NotEmpty()
                .WithError(ChangePasswordCommandErrors.ConfirmPasswordIsRequired)
            .MinimumLength(Domain.Constants.Constraints.User.PasswordMinLength)
                .WithError(ChangePasswordCommandErrors.ConfirmPasswordMinLength)
                .When(x => !string.IsNullOrWhiteSpace(x.ConfirmNewPassword), ApplyConditionTo.CurrentValidator)
            .MaximumLength(Domain.Constants.Constraints.User.PasswordMaxLength)
                .WithError(ChangePasswordCommandErrors.ConfirmPasswordMaxLength)
                .When(x => !string.IsNullOrWhiteSpace(x.ConfirmNewPassword), ApplyConditionTo.CurrentValidator)
            .Must(x => x.Any(char.IsUpper))
                .WithError(ChangePasswordCommandErrors.ConfirmPasswordFormatInvalidUpperCase)
                .When(x => !string.IsNullOrWhiteSpace(x.ConfirmNewPassword), ApplyConditionTo.CurrentValidator)
            .Must(x => x.Any(char.IsLower))
                .WithError(ChangePasswordCommandErrors.ConfirmPasswordFormatInvalidLowerCase)
                .When(x => !string.IsNullOrWhiteSpace(x.ConfirmNewPassword), ApplyConditionTo.CurrentValidator)
            .Must(x => x.Any(char.IsDigit))
                .WithError(ChangePasswordCommandErrors.ConfirmPasswordFormatInvalidNumber)
                .When(x => !string.IsNullOrWhiteSpace(x.ConfirmNewPassword), ApplyConditionTo.CurrentValidator)
            .Matches(Domain.Constants.Constraints.User.PasswordFormat)
                .WithError(ChangePasswordCommandErrors.ConfirmPasswordFormatNonAlphanumeric)
                .When(x => !string.IsNullOrWhiteSpace(x.ConfirmNewPassword), ApplyConditionTo.CurrentValidator)
            .Equal(x => x.NewPassword)
                .WithError(ChangePasswordCommandErrors.ConfirmPasswordNotEquals)
                .When(x => !string.IsNullOrWhiteSpace(x.ConfirmNewPassword), ApplyConditionTo.CurrentValidator);
    }
}   