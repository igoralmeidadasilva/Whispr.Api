using FluentValidation;

namespace Whispr.Presentation.Web.Pages.Public.ForgotPassword;

public sealed class CreatePasswordRecoveryCodeModelValidator : AbstractValidator<CreatePasswordRecoveryCodeModel>
{
    public CreatePasswordRecoveryCodeModelValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
                .WithMessage("Email is required.")
            .EmailAddress()
                .WithMessage("Invalid email format.");
    }
}