namespace Whispr.Application.Features.V1.Auth.Commands.Logout;

public sealed record LogoutCommand : ICommand<Unit>
{
    public required string RefreshToken { get; init; }
}