namespace Whispr.Application.Core.Dtos.V1;

public sealed record UserDto
{
    public Guid? Id { get; init; }
    public string? Name { get; init; }
    public string? Email { get; init; }
}