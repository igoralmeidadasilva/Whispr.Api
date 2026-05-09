namespace Whispr.Application.Features.V1.Users.Commands.Delete;

public sealed record DeleteUserCommand : ICommand<Unit>
{
    public required Guid UserId { get; init; }
}