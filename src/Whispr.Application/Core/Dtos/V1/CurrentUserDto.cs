namespace Whispr.Application.Core.Dtos.V1;

public sealed record CurrentUserDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
}