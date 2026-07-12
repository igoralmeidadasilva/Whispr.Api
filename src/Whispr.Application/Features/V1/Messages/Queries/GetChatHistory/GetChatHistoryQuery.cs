using Whispr.Application.Core.Dtos.V1;
using Whispr.SharedKernel.Pagination;

namespace Whispr.Application.Features.V1.Messages.Queries.GetChatHistory;

public sealed record GetChatHistoryQuery : IQuery<PagedList<MessageDto>>
{
    public int PageNumber { get; init; } = Constants.Pagination.DefaultPageNumber;
    public int PageSize { get; init; } = Constants.Pagination.DefaultPageSize;  
}