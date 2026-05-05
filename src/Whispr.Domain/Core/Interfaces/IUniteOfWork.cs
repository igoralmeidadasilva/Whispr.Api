namespace Whispr.Domain.Core.Interfaces;

public interface IUniteOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken = default);    
}