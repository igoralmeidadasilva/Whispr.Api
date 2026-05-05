namespace Whispr.Presentation.Web.Core.Dtos;

public sealed record UserDto
{
    public required Guid Id { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
}