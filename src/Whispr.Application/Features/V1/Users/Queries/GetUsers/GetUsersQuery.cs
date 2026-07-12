using Whispr.Application.Core.Dtos.V1;
using Whispr.SharedKernel.Pagination;

namespace Whispr.Application.Features.V1.Users.Queries.GetUsers;

public sealed record GetUsersQuery : IQuery<PagedList<UserDto>>
{
    public int PageNumber { get; init; } = Constants.Pagination.DefaultPageNumber;
    public int PageSize { get; init; } = Constants.Pagination.DefaultPageSize;
}