namespace Whispr.Application.Features.V1.Users.Commands.Create;

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
                .WithError(CreateUserCommandErrors.NameIsRequired)
            .MinimumLength(Domain.Constants.Constraints.User.NameMinLength)
                .WithError(CreateUserCommandErrors.NameMinLength)
                .When(x => !string.IsNullOrWhiteSpace(x.Username), ApplyConditionTo.CurrentValidator)
            .MaximumLength(Domain.Constants.Constraints.User.NameMaxLength)
                .WithError(CreateUserCommandErrors.NameMaxLength)
                .When(x => !string.IsNullOrWhiteSpace(x.Username), ApplyConditionTo.CurrentValidator);

        RuleFor(x => x.Email)
            .NotEmpty()
                .WithError(CreateUserCommandErrors.EmailIsRequired)
            .EmailAddress()
                .WithError(CreateUserCommandErrors.EmailFormat)
                .When(x => !string.IsNullOrWhiteSpace(x.Email), ApplyConditionTo.CurrentValidator); ;

        RuleFor(x => x.Password)
            .NotEmpty()
                .WithError(CreateUserCommandErrors.PasswordIsRequired)
            .MinimumLength(Domain.Constants.Constraints.User.PasswordMinLength)
                .WithError(CreateUserCommandErrors.PasswordMinLength)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator)
            .MaximumLength(Domain.Constants.Constraints.User.PasswordMaxLength)
                .WithError(CreateUserCommandErrors.PasswordMaxLength)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator)
            .Must(x => x.Any(char.IsUpper))
                .WithError(CreateUserCommandErrors.PasswordFormatInvalidUpperCase)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator)
            .Must(x => x.Any(char.IsLower))
                .WithError(CreateUserCommandErrors.PasswordFormatInvalidLowerCase)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator)
            .Must(x => x.Any(char.IsDigit))
                .WithError(CreateUserCommandErrors.PasswordFormatInvalidNumber)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator)
            .Matches(Domain.Constants.Constraints.User.PasswordFormat)
                .WithError(CreateUserCommandErrors.PasswordFormatNonAlphanumeric)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator);

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
                .WithError(CreateUserCommandErrors.ConfirmPasswordIsRequired)
            .MinimumLength(Domain.Constants.Constraints.User.PasswordMinLength)
                .WithError(CreateUserCommandErrors.ConfirmPasswordMinLength)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator)
            .MaximumLength(Domain.Constants.Constraints.User.PasswordMaxLength)
                .WithError(CreateUserCommandErrors.ConfirmPasswordMaxLength)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator)
            .Must(x => x.Any(char.IsUpper))
                .WithError(CreateUserCommandErrors.ConfirmPasswordFormatInvalidUpperCase)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator)
            .Must(x => x.Any(char.IsLower))
                .WithError(CreateUserCommandErrors.ConfirmPasswordFormatInvalidLowerCase)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator)
            .Must(x => x.Any(char.IsDigit))
                .WithError(CreateUserCommandErrors.ConfirmPasswordFormatInvalidNumber)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator)
            .Matches(Domain.Constants.Constraints.User.PasswordFormat)
                .WithError(CreateUserCommandErrors.ConfirmPasswordFormatNonAlphanumeric)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator)
            .Equal(x => x.Password)
                .WithError(CreateUserCommandErrors.ConfirmPasswordNotEquals)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator);
    }
}