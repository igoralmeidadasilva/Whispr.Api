using Microsoft.EntityFrameworkCore;
using Whispr.Domain.Features.Entities.RefreshTokens;
using Whispr.Infrastructure.Core.Data.Context;

namespace Whispr.Infrastructure.Features.Repositories.ReadOnly;

public sealed class RefreshTokenReadOnlyRepository : BaseReadOnlyRepository<RefreshToken>, IRefreshTokenReadOnlyRepository
{
    public RefreshTokenReadOnlyRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default)
    {
        return await Context
            .RefreshTokens
            .AsNoTracking()
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.TokenHash.Value == tokenHash, cancellationToken);
    }
}