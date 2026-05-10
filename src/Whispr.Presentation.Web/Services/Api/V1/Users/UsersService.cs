using Microsoft.AspNetCore.WebUtilities;
using Whispr.Presentation.Web.Core.Dtos;
using Whispr.Presentation.Web.Core.Http;
using Whispr.Presentation.Web.Core.Models;
using Whispr.Presentation.Web.Services.Api.V1.Users.Requests;

namespace Whispr.Presentation.Web.Services.Api.V1.Users;

public sealed class UsersService : IUsersService
{
    private readonly IApiClient _apiClient;

    public UsersService(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<ApiResponse<NoContent>> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        return await _apiClient.PostAsync(Routes.Api.Users.Create, request, cancellationToken);
    }

    public async Task<ApiResponse<NoContent>> DeleteAsync(DeleteUserRequest request, CancellationToken cancellationToken = default)
    {
        var url = Routes.Api.Users.Delete.Replace("{userId:Guid}", request.UserId.ToString());

        return await _apiClient.DeleteAsync(url, cancellationToken);
    }

    public async Task<ApiResponse<NoContent>> UpdateAsync(UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        var url = Routes.Api.Users.Update.Replace("{userId:Guid}", request.UserId.ToString());

        return await _apiClient.PutAsync(url, request, cancellationToken);
    }

    public async Task<ApiResponse<PagedModel<UserDto>>> GetAllAsync(GetUsersRequest request, CancellationToken cancellationToken = default)
    {
        var url = QueryHelpers.AddQueryString(Routes.Api.Users.GetAll, new Dictionary<string, string?>
        {
            ["pageNumber"] = request.PageNumber.ToString(),
            ["pageSize"] = request.PageSize.ToString()
        });

        return await _apiClient.GetAsync<PagedModel<UserDto>>(url, cancellationToken);
    }

    public async Task<ApiResponse<UserDto>> GetByIdAsync(GetUserByIdRequest request, CancellationToken cancellationToken = default)
    {
        var url = Routes.Api.Users.GetById.Replace("{userId:Guid}", request.UserId.ToString());

        return await _apiClient.GetAsync<UserDto>(url, cancellationToken);
    }
}