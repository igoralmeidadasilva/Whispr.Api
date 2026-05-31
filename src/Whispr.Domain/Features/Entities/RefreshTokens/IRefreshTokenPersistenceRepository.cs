using Whispr.Domain.Core.Repositories;

namespace Whispr.Domain.Features.Entities.RefreshTokens;

public interface IRefreshTokenPersistenceRepository : IPersistenceRepository<RefreshToken>
{
}