namespace Whispr.Domain.Core.Interfaces;

public interface ISoftDeletable
{
    bool IsDeleted { get; }
    DateTime? DeletedAtUtc { get; }
    void Delete();
    void Restore();
}