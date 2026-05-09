namespace Whispr.Application.Features.V1.Users.Commands.Create;

public sealed record CreateUserCommand : ICommand<Unit>
{
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}