namespace Whispr.Domain.Entities;

public sealed class Message
{
    public required Guid Id { get; set; }
    public required string SenderId { get; set; }
    public required User Sender { get; set; }
    public required string Content { get; set; }
    public required DateTime CreatedAtUtc { get; set; }
}