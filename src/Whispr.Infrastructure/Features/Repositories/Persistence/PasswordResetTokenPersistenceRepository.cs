using Microsoft.EntityFrameworkCore;
using Whispr.Domain.Features.Entities.PasswordResetTokens;
using Whispr.Infrastructure.Core.Data.Context;

namespace Whispr.Infrastructure.Features.Repositories.Persistence;

internal sealed class PasswordResetTokenPersistenceRepository : BasePersistenceRepository<PasswordResetToken>, IPasswordResetTokenPersistenceRepository
{
    public PasswordResetTokenPersistenceRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<PasswordResetToken>> GetAllActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var tokens = await Context.PasswordResetTokens
            .Where(t => t.UserId == userId)
            .ToListAsync(cancellationToken);

        return tokens.Where(t => t.IsValid);
    }
}