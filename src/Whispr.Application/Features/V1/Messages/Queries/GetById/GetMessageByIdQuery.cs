using Whispr.Application.Core.Dtos.V1;

namespace Whispr.Application.Features.V1.Messages.Queries.GetById;

public sealed record GetMessageByIdQuery : IQuery<MessageDto>
{
    public required Guid MessageId { get; init; }
}