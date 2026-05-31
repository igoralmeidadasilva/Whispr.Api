using Whispr.Domain.Core.Repositories;

namespace Whispr.Domain.Features.Entities.PasswordResetTokens;

public interface IPasswordResetTokenReadOnlyRepository : IReadOnlyRepository<PasswordResetToken>
{
    Task<PasswordResetToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default);
    Task<PasswordResetToken?> GetLatestActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}