using Microsoft.EntityFrameworkCore;
using Whispr.Domain.Features.Entities.RefreshTokens;
using Whispr.Infrastructure.Core.Data.Context;

namespace Whispr.Infrastructure.Features.Repositories.Persistence;

internal sealed class RefreshTokenPersistenceRepository : BasePersistenceRepository<RefreshToken>, IRefreshTokenPersistenceRepository
{
    public RefreshTokenPersistenceRepository(ApplicationDbContext context) : base(context)
    {
    }
}