using MediatR;
using System.Text.Json.Serialization;
using Whispr.Application.Core.Abstractions;

namespace Whispr.Application.Features.V1.Users.Commands.Update;

public sealed record UpdateUserCommand : ICommand<Unit>
{
    [JsonIgnore]
    public Guid UserId { get; init; }
    public required string UserName { get; init; }
    public required string Email { get; init; }
}