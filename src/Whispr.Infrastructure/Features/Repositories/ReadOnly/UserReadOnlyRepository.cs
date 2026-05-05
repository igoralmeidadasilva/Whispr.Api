using Microsoft.EntityFrameworkCore;
using Whispr.Domain.Features.Entities.User;
using Whispr.Infrastructure.Core.Data.Context;

namespace Whispr.Infrastructure.Features.Repositories.ReadOnly;

public sealed class UserReadOnlyRepository : BaseReadOnlyRepository<User>, IUserReadOnlyRepository
{
    public UserReadOnlyRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await Context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    public async Task<User?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await Context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }
}