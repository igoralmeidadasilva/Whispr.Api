using Whispr.Domain.Core.Repositories;

namespace Whispr.Domain.Features.Entities.Messages;

public interface IMessageReadOnlyRepository : IReadOnlyRepository<Message>
{
    Task<Message?> GetByIdWithAttachmentsAsync(Guid id, CancellationToken cancellationToken = default);
}