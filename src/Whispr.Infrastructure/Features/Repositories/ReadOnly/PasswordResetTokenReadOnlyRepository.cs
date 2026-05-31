using Microsoft.EntityFrameworkCore;
using Whispr.Domain.Features.Entities.PasswordResetTokens;
using Whispr.Domain.Features.Entities.RefreshTokens;
using Whispr.Infrastructure.Core.Data.Context;

namespace Whispr.Infrastructure.Features.Repositories.ReadOnly;

internal sealed class PasswordResetTokenReadOnlyRepository : BaseReadOnlyRepository<PasswordResetToken>, IPasswordResetTokenReadOnlyRepository
{
    public PasswordResetTokenReadOnlyRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PasswordResetToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default)
    {
        return await Context
            .PasswordResetTokens
            .AsNoTracking()
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.TokenHash.Value == tokenHash, cancellationToken);
    }

    public async Task<PasswordResetToken?> GetLatestActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await Context
            .PasswordResetTokens
            .AsNoTracking()
            .Where(x => !x.UsedAtUtc.HasValue)
            .OrderByDescending(l => l.CreatedAtUtc)
            .FirstOrDefaultAsync(rt => rt.UserId == userId, cancellationToken);
    }
}