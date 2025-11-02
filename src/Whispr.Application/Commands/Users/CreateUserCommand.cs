using Whispr.Application.Core.Abstractions;
using Whispr.SharedKernel.Results;

namespace Whispr.Application.Commands.Users;

public sealed record CreateUserCommand : ICommand<Result<Guid>>
{
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}