using FluentValidation;

namespace Whispr.Application.Features.V1.Users.Commands.Create;

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage(CreateUserCommandValidationErrors.UserNameIsRequired)
            .MinimumLength(Domain.Constants.Constraints.User.UserNameMinLength)
            .WithMessage(CreateUserCommandValidationErrors.UserNameMinLength)
            .When(x => !string.IsNullOrWhiteSpace(x.Username), ApplyConditionTo.CurrentValidator)
            .MaximumLength(Domain.Constants.Constraints.User.UserNameMaxLength)
            .WithMessage(CreateUserCommandValidationErrors.UserNameMaxLength)
            .When(x => !string.IsNullOrWhiteSpace(x.Username), ApplyConditionTo.CurrentValidator);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(CreateUserCommandValidationErrors.EmailIsRequired)
            .EmailAddress()
            .WithMessage(CreateUserCommandValidationErrors.EmailFormat);

        RuleFor(x => x.Password)
            .NotEmpty()
                .WithMessage(CreateUserCommandValidationErrors.PasswordIsRequired)
            .MinimumLength(Domain.Constants.Constraints.User.PasswordMinLength)
                .WithMessage(CreateUserCommandValidationErrors.PasswordMinLength)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator)
            .MaximumLength(Domain.Constants.Constraints.User.PasswordMaxLength)
                .WithMessage(CreateUserCommandValidationErrors.PasswordMaxLength)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator)
            .Must(x => x.Any(char.IsUpper))
                .WithMessage(CreateUserCommandValidationErrors.PasswordFormatInvalidUpperCase)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator)
            .Must(x => x.Any(char.IsLower))
                .WithMessage(CreateUserCommandValidationErrors.PasswordFormatInvalidLowerCase)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator)
            .Must(x => x.Any(char.IsDigit))
                .WithMessage(CreateUserCommandValidationErrors.PasswordFormatInvalidNumber)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator)
            .Matches(Domain.Constants.Constraints.User.PasswordFormat)
                .WithMessage(CreateUserCommandValidationErrors.PasswordFormatNonAlphanumeric)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator);
    }
}