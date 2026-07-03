namespace Whispr.Application.Features.V1.Messages.Commands.Create;

public sealed record CreateMessageCommand : ICommand<Unit>
{
    public required Guid UserId { get; init; }
    public required string Content { get; init; }
}