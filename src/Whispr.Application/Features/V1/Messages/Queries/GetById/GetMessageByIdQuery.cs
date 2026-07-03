using Whispr.Application.Core.Models.V1;

namespace Whispr.Application.Features.V1.Messages.Queries.GetById;

public sealed record GetMessageByIdQuery : IQuery<MessageDto>
{
    public required Guid MessageId { get; init; }
}