using Whispr.Domain.Core.Repositories;

namespace Whispr.Domain.Features.Entities.Users;

public interface IUserPersistenceRepository : IPersistenceRepository<User>;