using Microsoft.EntityFrameworkCore;
using Whispr.Domain.Features.Entities.RefreshToken;
using Whispr.Infrastructure.Core.Data.Context;

namespace Whispr.Infrastructure.Features.Repositories.Persistence;

public sealed class RefreshTokenPersistenceRepository : BasePersistenceRepository<RefreshToken>, IRefreshTokenPersistenceRepository
{
    public RefreshTokenPersistenceRepository(ApplicationDbContext context) : base(context)
    {
    }
}