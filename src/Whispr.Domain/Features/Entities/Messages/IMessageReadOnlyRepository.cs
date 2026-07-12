using Whispr.Domain.Core.Repositories;
using Whispr.Domain.Features.Models;
using Whispr.SharedKernel.Pagination;

namespace Whispr.Domain.Features.Entities.Messages;

public interface IMessageReadOnlyRepository : IReadOnlyRepository<Message>
{
    Task<Message?> GetByIdWithAttachmentsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedList<Message>> GetFilteredMessagesPaginatedAsync(MessagePaginatedSearchParameters parameters, CancellationToken cancellationToken = default);
}