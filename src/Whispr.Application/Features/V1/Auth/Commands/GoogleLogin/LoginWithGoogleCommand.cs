using Whispr.Application.Core.Models.V1;

namespace Whispr.Application.Features.V1.Auth.Commands.GoogleLogin;

public sealed record LoginWithGoogleCommand : ICommand<AuthTokenDto>
{
    public required string IdToken { get; set; }
}