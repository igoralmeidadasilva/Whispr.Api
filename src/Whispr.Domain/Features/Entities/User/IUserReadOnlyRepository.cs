using Whispr.Domain.Core.Repositories;

namespace Whispr.Domain.Features.Entities.User;

public interface IUserReadOnlyRepository : IReadOnlyRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByNameAsync(string username, CancellationToken cancellationToken = default);
}