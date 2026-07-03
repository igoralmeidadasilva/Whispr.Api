using Whispr.Domain.Core.Repositories;

namespace Whispr.Domain.Features.Entities.Messages;

public interface IMessageReadOnlyRepository : IReadOnlyRepository<Message>;