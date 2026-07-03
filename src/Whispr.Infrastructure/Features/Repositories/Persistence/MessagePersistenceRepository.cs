using Whispr.Domain.Features.Entities.Messages;
using Whispr.Infrastructure.Core.Data.Context;

namespace Whispr.Infrastructure.Features.Repositories.Persistence;

internal sealed class MessagePersistenceRepository : BasePersistenceRepository<Message>, IMessagePersistenceRepository
{
    public MessagePersistenceRepository(ApplicationDbContext context) : base(context)
    {
    }
}