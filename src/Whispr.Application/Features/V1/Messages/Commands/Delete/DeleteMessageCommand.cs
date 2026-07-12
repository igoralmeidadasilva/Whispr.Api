namespace Whispr.Application.Features.V1.Messages.Commands.Delete;

public sealed record DeleteMessageCommand : ICommand<Unit>
{
    public required Guid MessageId { get; init; }
}