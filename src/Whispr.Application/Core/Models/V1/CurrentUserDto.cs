namespace Whispr.Application.Core.Models.V1;

public sealed record CurrentUserDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
}