using Microsoft.Extensions.Options;
using Whispr.Application.Core.Dtos.V1;
using Whispr.Application.Core.Mappings;
using Whispr.Application.Core.Options;
using Whispr.Application.Core.Services;
using Whispr.Domain.Core.Enums;
using Whispr.Domain.Features.Entities.Messages;
using Whispr.Domain.Features.Models;
using Whispr.SharedKernel.Pagination;

namespace Whispr.Application.Features.V1.Messages.Queries.GetChatHistory;

public sealed class GetChatHistoryQueryHandler : IQueryHandler<GetChatHistoryQuery, PagedList<MessageDto>>
{
    private readonly IMessageReadOnlyRepository _messageReadOnlyRepository;
    private readonly IStorageService _storageService;
    private readonly StorageOptions _storageOptions;

    public GetChatHistoryQueryHandler(IMessageReadOnlyRepository messageReadOnlyRepository, IStorageService storageService, IOptions<StorageOptions> storageOptions)
    {
        _messageReadOnlyRepository = messageReadOnlyRepository;
        _storageService = storageService;
        _storageOptions = storageOptions.Value;
    }

    public async Task<Result<PagedList<MessageDto>>> Handle(GetChatHistoryQuery request, CancellationToken cancellationToken)
    {
        var parameters = new MessagePaginatedSearchParameters
        {
            WithAttachments = true,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            SortDirection = SortDirection.Descending
        };

        PagedList<Message> messages = await _messageReadOnlyRepository.GetFilteredMessagesPaginatedAsync(parameters, cancellationToken);

        if (!messages.Items.Any())
        {
            return Result<PagedList<MessageDto>>.Success(PagedList<MessageDto>.Empty());
        }

        IEnumerable<MessageDto> messagesDto = messages.Items
            .Select(message =>
            {
                var attachments = message.Attachments.Select(attachment =>
                {
                    Result<string> result = _storageService.GetSasUri(
                        _storageOptions.ContainerName,
                        attachment.StorageKey,
                        TimeSpan.FromMinutes(Constants.Storage.DefaultTimeExpirationInMinutes));
            
                    return MessageAttachmentMappings.ToMessageAttachmentDto(attachment, result.Value!);
                });

                return MessageMappings.ToMessageDtoWithAttachments(message, attachments);
            });

        PagedList<MessageDto> page = new(
            messagesDto.ToList(),
            messages.TotalCount,
            messages.PageNumber,
            messages.PageSize);

        return Result<PagedList<MessageDto>>.Success(page);
    }
}