using Whispr.Domain.Core.Repositories;

namespace Whispr.Domain.Features.Entities.PasswordResetTokens;

public interface IPasswordResetTokenPersistenceRepository : IPersistenceRepository<PasswordResetToken>
{
    Task<IEnumerable<PasswordResetToken>> GetAllActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}