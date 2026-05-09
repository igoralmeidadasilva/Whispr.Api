namespace Whispr.Application.Features.V1.Auth.Commands.Logout;

public sealed class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
                .WithError(LogoutCommandErrors.RefreshTokenIsRequired);
    }
}