using FluentValidation;
using Whispr.Application.Core.Extensions;

namespace Whispr.Application.Features.V1.Auth.Commands.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
                .WithError(LoginCommandErrors.EmailIsRequired)
            .EmailAddress()
                .WithError(LoginCommandErrors.EmailFormat);

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

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
                .WithError(LoginCommandErrors.ConfirmPasswordIsRequired)
            .MinimumLength(Domain.Constants.Constraints.User.PasswordMinLength)
                .WithError(LoginCommandErrors.ConfirmPasswordMinLength)
                .When(x => !string.IsNullOrWhiteSpace(x.ConfirmPassword), ApplyConditionTo.CurrentValidator)
            .MaximumLength(Domain.Constants.Constraints.User.PasswordMaxLength)
                .WithError(LoginCommandErrors.ConfirmPasswordMaxLength)
                .When(x => !string.IsNullOrWhiteSpace(x.ConfirmPassword), ApplyConditionTo.CurrentValidator)
            .Must(x => x.Any(char.IsUpper))
                .WithError(LoginCommandErrors.ConfirmPasswordFormatInvalidUpperCase)
                .When(x => !string.IsNullOrWhiteSpace(x.ConfirmPassword), ApplyConditionTo.CurrentValidator)
            .Must(x => x.Any(char.IsLower))
                .WithError(LoginCommandErrors.ConfirmPasswordFormatInvalidLowerCase)
                .When(x => !string.IsNullOrWhiteSpace(x.ConfirmPassword), ApplyConditionTo.CurrentValidator)
            .Must(x => x.Any(char.IsDigit))
                .WithError(LoginCommandErrors.ConfirmPasswordFormatInvalidNumber)
                .When(x => !string.IsNullOrWhiteSpace(x.ConfirmPassword), ApplyConditionTo.CurrentValidator)
            .Matches(Domain.Constants.Constraints.User.PasswordFormat)
                .WithError(LoginCommandErrors.ConfirmPasswordFormatNonAlphanumeric)
                .When(x => !string.IsNullOrWhiteSpace(x.ConfirmPassword), ApplyConditionTo.CurrentValidator)
            .Equal(x => x.Password)
                .WithError(LoginCommandErrors.ConfirmPasswordDoesNotMatch);
    }
}