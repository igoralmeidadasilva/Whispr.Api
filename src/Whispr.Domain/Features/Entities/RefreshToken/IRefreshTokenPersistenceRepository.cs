using Whispr.Domain.Core.Repositories;

namespace Whispr.Domain.Features.Entities.RefreshToken;

public interface IRefreshTokenPersistenceRepository : IPersistenceRepository<RefreshToken>
{
}