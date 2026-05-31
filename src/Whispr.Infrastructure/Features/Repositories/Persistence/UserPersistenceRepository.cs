using Whispr.Domain.Features.Entities.Users;
using Whispr.Infrastructure.Core.Data.Context;

namespace Whispr.Infrastructure.Features.Repositories.Persistence;

public sealed class UserPersistenceRepository : BasePersistenceRepository<User>, IUserPersistenceRepository
{
    public UserPersistenceRepository(ApplicationDbContext context) : base(context)
    {
    }
}