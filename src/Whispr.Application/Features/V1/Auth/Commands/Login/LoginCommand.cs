using Whispr.Application.Core.Models.V1;

namespace Whispr.Application.Features.V1.Auth.Commands.Login;

public sealed record LoginCommand : ICommand<AuthTokenDto>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}