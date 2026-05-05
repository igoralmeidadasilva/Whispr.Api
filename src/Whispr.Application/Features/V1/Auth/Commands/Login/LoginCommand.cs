using Whispr.Application.Core.Abstractions;
using Whispr.Application.Core.Models.V1;
using Whispr.Domain.Features.Models;

namespace Whispr.Application.Features.V1.Auth.Commands.Login;

public class LoginCommand : ICommand<AuthTokenDto>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string ConfirmPassword { get; init; }
}