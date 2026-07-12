using Microsoft.EntityFrameworkCore;
using Whispr.Domain.Core.Enums;
using Whispr.Domain.Features.Entities.Messages;
using Whispr.Domain.Features.Models;
using Whispr.Infrastructure.Core.Data.Context;
using Whispr.Infrastructure.Core.Extensions;
using Whispr.SharedKernel.Pagination;

namespace Whispr.Infrastructure.Features.Repositories.ReadOnly;

internal sealed class MessageReadOnlyRepository : BaseReadOnlyRepository<Message>, IMessageReadOnlyRepository
{
    public MessageReadOnlyRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Message?> GetByIdWithAttachmentsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Context.Messages.AsNoTracking().Include(x => x.Attachments).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<PagedList<Message>> GetFilteredMessagesPaginatedAsync(MessagePaginatedSearchParameters parameters, CancellationToken cancellationToken = default)
    {
        IQueryable<Message> query = Context.Messages.AsNoTracking();

        if (parameters.WithAttachments)
        {
            query = query.Include(x => x.Attachments);
        }

        if (parameters.SortDirection == SortDirection.Ascending)
        {
            query = query.OrderBy(x => x.CreatedAtUtc);
        }
        else
        {
            query = query.OrderByDescending(x => x.CreatedAtUtc);
        }

        return await query.ToPagedListAsync(parameters.PageNumber, parameters.PageSize, cancellationToken);
    }
}