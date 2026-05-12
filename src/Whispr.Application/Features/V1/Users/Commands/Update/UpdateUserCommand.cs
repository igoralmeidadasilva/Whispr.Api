using System.Text.Json.Serialization;

namespace Whispr.Application.Features.V1.Users.Commands.Update;

public sealed record UpdateUserCommand : ICommand<Unit>
{
    [JsonIgnore]
    public Guid UserId { get; init; }
    public required string UserName { get; init; }
    public required string Email { get; init; }
}