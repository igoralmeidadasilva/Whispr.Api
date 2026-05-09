namespace Whispr.Application.Features.V1.Auth.Commands.GoogleLogin;

public sealed class LoginWithGoogleCommandValidator : AbstractValidator<LoginWithGoogleCommand>
{
    public LoginWithGoogleCommandValidator()
    {
        RuleFor(x => x.IdToken)
            .NotEmpty()
                .WithError(LoginWithGoogleCommandErrors.IdTokenIsRequired);
    }
}