using Whispr.Domain.Core.Repositories;

namespace Whispr.Domain.Features.Entities.RefreshToken;

public interface IRefreshTokenReadOnlyRepository : IReadOnlyRepository<RefreshToken>
{
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
}