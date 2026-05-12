namespace Whispr.Presentation.Web.Services.Api.V1.Users.Requests;

public sealed record GetUsersRequest
{
    public int PageNumber { get; init; } = Constants.Pagination.DefaultPageNumber;
    public int PageSize { get; init; } = Constants.Pagination.DefaultPageSize;
}