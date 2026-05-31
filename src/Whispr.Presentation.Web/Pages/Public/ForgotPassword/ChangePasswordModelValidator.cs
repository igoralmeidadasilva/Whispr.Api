using FluentValidation;

namespace Whispr.Presentation.Web.Pages.Public.ForgotPassword;

public sealed class ChangePasswordModelValidator : AbstractValidator<ChangePasswordModel>
{
    public ChangePasswordModelValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
                .WithMessage("Email is required.")
            .EmailAddress()
                .WithMessage("Invalid email format.");

        RuleFor(x => x.RecoveryCode)
            .NotEmpty()
                .WithMessage("All digits are required.")
            .Length(4)
                .WithMessage("All four digits must be filled.")
            .Matches(@"^\d{4}$")
                .WithMessage("All digits must be numeric.");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
                .WithMessage("Password is required.")
            .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.")
            .MaximumLength(100)
                .WithMessage("Password must be at most 100 characters long.");

        RuleFor(x => x.ConfirmNewPassword)
            .NotEmpty()
                .WithMessage("Confirm password is required.")
            .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.")
            .MaximumLength(100)
                .WithMessage("Password must be at most 100 characters long.")
            .Equal(x => x.NewPassword)
                .WithMessage("Passwords do not match.");
    }
}