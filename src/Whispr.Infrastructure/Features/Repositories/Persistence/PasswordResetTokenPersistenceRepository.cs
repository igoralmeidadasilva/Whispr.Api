using Whispr.Domain.Features.Entities.PasswordResetTokens;
using Whispr.Infrastructure.Core.Data.Context;

namespace Whispr.Infrastructure.Features.Repositories.Persistence;

internal sealed class PasswordResetTokenPersistenceRepository : BasePersistenceRepository<PasswordResetToken>, IPasswordResetTokenPersistenceRepository
{
    public PasswordResetTokenPersistenceRepository(ApplicationDbContext context) : base(context)
    {
    }
}