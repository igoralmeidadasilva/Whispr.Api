using System.Text.Json.Serialization;

namespace Whispr.Application.Features.V1.Messages.Commands.Update;

public sealed record UpdateMessageCommand : ICommand<Unit>
{
    [JsonIgnore]
    public Guid MessageId { get; init; }
    public required string Content { get; init; }
}