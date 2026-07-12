using Whispr.Application.Core.Dtos.V1;

namespace Whispr.Application.Features.V1.Auth.Commands.Refresh;

public sealed record RefreshCommand : ICommand<AuthTokenDto>
{
    public required string RefreshToken { get; init; }
}