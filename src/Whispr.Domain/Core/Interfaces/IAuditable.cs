namespace Whispr.Domain.Core.Interfaces;

public interface IAuditable
{
    DateTime CreatedAtUtc { get; }
    DateTime? UpdatedAtUtc { get; }
}