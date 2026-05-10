namespace Whispr.Application.Features.V1.Auth.Commands.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
                .WithError(LoginCommandErrors.EmailIsRequired)
            .EmailAddress()
                .WithError(LoginCommandErrors.EmailFormat)
                .When(x => !string.IsNullOrWhiteSpace(x.Email), ApplyConditionTo.CurrentValidator);

        RuleFor(x => x.Password)
            .NotEmpty()
                .WithError(LoginCommandErrors.PasswordIsRequired)
            .MinimumLength(Domain.Constants.Constraints.User.PasswordMinLength)
                .WithError(LoginCommandErrors.PasswordMinLength)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator)
            .MaximumLength(Domain.Constants.Constraints.User.PasswordMaxLength)
                .WithError(LoginCommandErrors.PasswordMaxLength)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator)
            .Must(x => x.Any(char.IsUpper))
                .WithError(LoginCommandErrors.PasswordFormatInvalidUpperCase)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator)
            .Must(x => x.Any(char.IsLower))
                .WithError(LoginCommandErrors.PasswordFormatInvalidLowerCase)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator)
            .Must(x => x.Any(char.IsDigit))
                .WithError(LoginCommandErrors.PasswordFormatInvalidNumber)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator)
            .Matches(Domain.Constants.Constraints.User.PasswordFormat)
                .WithError(LoginCommandErrors.PasswordFormatNonAlphanumeric)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator);
    }
}