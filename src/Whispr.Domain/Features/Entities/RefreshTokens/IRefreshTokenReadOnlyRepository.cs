using Whispr.Domain.Core.Repositories;

namespace Whispr.Domain.Features.Entities.RefreshTokens;

public interface IRefreshTokenReadOnlyRepository : IReadOnlyRepository<RefreshToken>
{
    Task<RefreshToken?> GetByTokenHashAsync(string token, CancellationToken cancellationToken = default);
}