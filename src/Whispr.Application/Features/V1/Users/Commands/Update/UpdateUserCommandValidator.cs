using FluentValidation;
using Whispr.Application.Core.Extensions;

namespace Whispr.Application.Features.V1.Users.Commands.Update;

public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
                .WithError(UpdateUserCommandErrors.UserNameIsRequired)
            .MinimumLength(Domain.Constants.Constraints.User.UserNameMinLength)
                .WithError(UpdateUserCommandErrors.UserNameMinLength)
                .When(x => !string.IsNullOrWhiteSpace(x.UserName), ApplyConditionTo.CurrentValidator)
            .MaximumLength(Domain.Constants.Constraints.User.UserNameMaxLength)
                .WithError(UpdateUserCommandErrors.UserNameMaxLength)
                .When(x => !string.IsNullOrWhiteSpace(x.UserName), ApplyConditionTo.CurrentValidator);

        RuleFor(x => x.Email)
            .NotEmpty()
                .WithError(UpdateUserCommandErrors.EmailIsRequired)
            .EmailAddress()
                .WithError(UpdateUserCommandErrors.EmailFormat);
    }
}