using Whispr.Application.Core.Mappings;
using Whispr.Application.Core.Dtos.V1;
using Whispr.Domain.Features.Entities.Messages;

namespace Whispr.Application.Features.V1.Messages.Queries.GetById;

internal sealed class GetMessageByIdQueryHandler : IQueryHandler<GetMessageByIdQuery, MessageDto>
{
    private readonly IMessageReadOnlyRepository _messageReadOnlyRepository;

    public GetMessageByIdQueryHandler(IMessageReadOnlyRepository messageReadOnlyRepository)
    {
        _messageReadOnlyRepository = messageReadOnlyRepository;
    }

    public async Task<Result<MessageDto>> Handle(GetMessageByIdQuery request, CancellationToken cancellationToken)
    {
        Message? message = await _messageReadOnlyRepository.GetByIdAsync(request.MessageId, cancellationToken);
        if (message is null)
        {
            return Result<MessageDto>.Failure(GetMessageByIdQueryErrors.MessageNotFound);
        }

        MessageDto messageDto = MessageMappings.ToMessageDto(message);

        return Result<MessageDto>.Success(messageDto);
    }
}