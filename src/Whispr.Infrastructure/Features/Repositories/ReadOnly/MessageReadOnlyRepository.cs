using Whispr.Domain.Features.Entities.Messages;
using Whispr.Infrastructure.Core.Data.Context;

namespace Whispr.Infrastructure.Features.Repositories.ReadOnly;

internal sealed class MessageReadOnlyRepository : BaseReadOnlyRepository<Message>, IMessageReadOnlyRepository
{
    public MessageReadOnlyRepository(ApplicationDbContext context) : base(context)
    {
    }
}