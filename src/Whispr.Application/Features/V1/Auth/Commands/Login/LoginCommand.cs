using Whispr.Application.Core.Abstractions;
using Whispr.Domain.Features.Models;

namespace Whispr.Application.Features.V1.Auth.Commands.Login;

public class LoginCommand : ICommand<TokenModel>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string ConfirmPassword { get; init; }
}