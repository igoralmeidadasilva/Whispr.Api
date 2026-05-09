using Whispr.Application.Core.Models.V1;

namespace Whispr.Application.Features.V1.Auth.Commands.Refresh;

public sealed record RefreshCommand : ICommand<AuthTokenDto>
{
    public required string ExpiredAccessToken { get; init; }
    public required string RefreshToken { get; init; }
}