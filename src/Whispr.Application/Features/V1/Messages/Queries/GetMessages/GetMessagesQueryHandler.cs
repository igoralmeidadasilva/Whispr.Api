using Whispr.Application.Core.Mappings;
using Whispr.Application.Core.Models.V1;
using Whispr.Domain.Features.Entities.Messages;
using Whispr.SharedKernel.Pagination;

namespace Whispr.Application.Features.V1.Messages.Queries.GetMessages;

internal sealed class GetMessagesQueryHandler : IQueryHandler<GetMessagesQuery, PagedList<MessageDto>>
{
    private readonly IMessageReadOnlyRepository _messageReadOnlyRepository;

    public GetMessagesQueryHandler(IMessageReadOnlyRepository messageReadOnlyRepository)
    {
        _messageReadOnlyRepository = messageReadOnlyRepository;
    }

    public async Task<Result<PagedList<MessageDto>>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        PagedList<Message> messages = await _messageReadOnlyRepository.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        if (!messages.Items.Any())
        {
            return Result<PagedList<MessageDto>>.Success(PagedList<MessageDto>.Empty());
        }

        List<MessageDto> messagesDto = messages.Items
            .Select(MessageMappings.ToMessageDto)
            .ToList();

        PagedList<MessageDto> page = new(
            messagesDto,
            messages.TotalCount,
            messages.PageNumber,
            messages.PageSize);

        return Result<PagedList<MessageDto>>.Success(page);
    }
}