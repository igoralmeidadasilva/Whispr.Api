using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Whispr.Application.Core.Extensions;
using Whispr.Domain.Entities;

namespace Whispr.Application.Commands.Users;

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private readonly UserManager<User> _userManager;
    
    public CreateUserCommandValidator(UserManager<User> userManager)
    {
        _userManager = userManager;

        RuleFor(x => x.Username)
            .NotEmpty()
                .WithError(CreateUserCommandValidationErrors.UserNameIsRequired)
            .MinimumLength(Domain.Constants.Constraints.User.UserNameMinLength)
                .WithError(CreateUserCommandValidationErrors.UserNameMinLength)
                .When(x => !string.IsNullOrWhiteSpace(x.Username), ApplyConditionTo.CurrentValidator)
            .MaximumLength(Domain.Constants.Constraints.User.UserNameMaxLength)
                .WithError(CreateUserCommandValidationErrors.UserNameMaxLength)
                .When(x => !string.IsNullOrWhiteSpace(x.Username), ApplyConditionTo.CurrentValidator)
            .DependentRules(() =>
            {
                RuleFor(x => x.Username)
                    .MustAsync(IsUserNameUnique)
                        .WithError(CreateUserCommandValidationErrors.UserNameAlreadyExists);
            });
        
        RuleFor(x => x.Email)
            .NotEmpty()
                .WithError(CreateUserCommandValidationErrors.EmailIsRequired)
            .EmailAddress()
                .WithError(CreateUserCommandValidationErrors.EmailFormat)
            .DependentRules(() =>
            {
                RuleFor(x => x.Email)
                    .MustAsync(IsEmailUnique)
                    .WithError(CreateUserCommandValidationErrors.EmailAlreadyExists);
            });

        RuleFor(x => x.Password)
            .NotEmpty()
                .WithError(CreateUserCommandValidationErrors.PasswordIsRequired)
            .MinimumLength(Domain.Constants.Constraints.User.PasswordMinLength)
                .WithError(CreateUserCommandValidationErrors.PasswordMinLength)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator)
            .MaximumLength(Domain.Constants.Constraints.User.PasswordMaxLength)
                .WithError(CreateUserCommandValidationErrors.PasswordMaxLength)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator)
            .Must(x => x.Any(char.IsUpper))
                .WithError(CreateUserCommandValidationErrors.PasswordFormatInvalidUpperCase)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator)
            .Must(x => x.Any(char.IsLower))
                .WithError(CreateUserCommandValidationErrors.PasswordFormatInvalidLowerCase)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator)
            .Must(x => x.Any(char.IsDigit))
                .WithError(CreateUserCommandValidationErrors.PasswordFormatInvalidNumber)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator)
            .Matches(Domain.Constants.Constraints.User.PasswordFormat)
                .WithError(CreateUserCommandValidationErrors.PasswordFormatNonAlphanumeric)
                .When(x => !string.IsNullOrWhiteSpace(x.Password), ApplyConditionTo.CurrentValidator);
    }

    private async Task<bool> IsEmailUnique(string email, CancellationToken cancellationToken)
    {
        User? user = await _userManager.FindByEmailAsync(email);
        return user == null;
    }
    
    private async Task<bool> IsUserNameUnique(string userName, CancellationToken cancellationToken)
    {
        User? user = await _userManager.FindByNameAsync(userName);
        return user == null;
    }
}