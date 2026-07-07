using Microsoft.EntityFrameworkCore;
using Whispr.Domain.Features.Entities.Messages;
using Whispr.Infrastructure.Core.Data.Context;

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
}