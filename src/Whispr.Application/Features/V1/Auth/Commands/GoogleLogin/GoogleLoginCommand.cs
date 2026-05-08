using Whispr.Application.Core.Abstractions;
using Whispr.Application.Core.Models.V1;

namespace Whispr.Application.Features.V1.Auth.Commands.GoogleLogin;

public sealed record GoogleLoginCommand : ICommand<AuthTokenDto>
{
    public required string GoogleId { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
}