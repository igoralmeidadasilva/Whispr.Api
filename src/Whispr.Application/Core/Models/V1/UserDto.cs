namespace Whispr.Application.Core.Models.V1;

public sealed record UserDto
{
    public Guid? Id { get; init; }
    public string? Username { get; init; }
    public string? Email { get; init; }
}