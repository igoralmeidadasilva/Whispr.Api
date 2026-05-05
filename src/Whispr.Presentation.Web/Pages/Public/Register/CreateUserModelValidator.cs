using FluentValidation;

namespace Whispr.Presentation.Web.Pages.Public.Register;

public sealed class CreateUserModelValidator : AbstractValidator<CreateUserModel>
{
    public CreateUserModelValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
                .WithMessage("Username is required.")
            .MinimumLength(3)
                .WithMessage("Username must be at least 3 characters long.")
            .MaximumLength(20)
                .WithMessage("Username must be at most 20 characters long.");
        RuleFor(x => x.Email)
            .NotEmpty()
                .WithMessage("Email is required.")
            .EmailAddress()
                .WithMessage("Invalid email format.");
        RuleFor(x => x.Password)
            .NotEmpty()
                .WithMessage("Password is required.")
            .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.")
            .MaximumLength(100)
                .WithMessage("Password must be at most 100 characters long.");
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password)
                .WithMessage("Passwords do not match.");
    }
}