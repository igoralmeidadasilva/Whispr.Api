namespace Whispr.Presentation.Api.Core.Models;

public sealed record CurrentUserModel
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
}