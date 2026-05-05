using Microsoft.EntityFrameworkCore;
using Whispr.Domain.Features.Entities.RefreshToken;
using Whispr.Infrastructure.Core.Data.Context;

namespace Whispr.Infrastructure.Features.Repositories.ReadOnly;

public sealed class RefreshTokenReadOnlyRepository : BaseReadOnlyRepository<RefreshToken>, IRefreshTokenReadOnlyRepository
{
    public RefreshTokenReadOnlyRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await Context
            .RefreshTokens
            .AsNoTracking()
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);
    }
}