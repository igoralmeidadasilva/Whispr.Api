using Whispr.Presentation.Web.Core.Dtos;
using Whispr.Presentation.Web.Core.Http;
using Whispr.Presentation.Web.Services.Api.V1.Users.Requests;
using Whispr.Presentation.Web.Services.Ui.Alert;

namespace Whispr.Presentation.Web.Services.Api.V1.Users;

public sealed class UsersService : IUsersService
{
    private readonly IApiClient _apiClient;
    private readonly IAlertService _alertService;

    public UsersService(IApiClient apiClient, IAlertService alertService)
    {
        _apiClient = apiClient;
        _alertService = alertService;
    }

    public async Task<NoContent?> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        var respose = await _apiClient.PostAsync<NoContent>("/api/v1/users", request, cancellationToken);
        if (respose.IsFailure)
        {
            await _alertService.ShowAsync(new()
            {
                Color = Core.Enums.Colors.Warning,
                Message = respose.ProblemDetails?.Detail ?? "An error occurred while creating the user.",
            });
            return null;
        }
        return respose.Value;
    }

    public Task<ApiResponse<NoContent>> DeleteAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<IEnumerable<UserDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<UserDto>> GetByIdAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<NoContent>> UpdateAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}