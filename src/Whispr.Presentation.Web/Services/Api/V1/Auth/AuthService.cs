using Whispr.Presentation.Web.Core.Dtos;
using Whispr.Presentation.Web.Services.Api.V1.Auth.Requests;
using Whispr.Presentation.Web.Services.Ui.Alert;

namespace Whispr.Presentation.Web.Services.Api.V1.Auth;

public sealed class AuthService : IAuthService
{
    private readonly IApiClient _apiClient;
    private readonly IAlertService _alertService;

    public AuthService(IApiClient apiClient, IAlertService alertService)
    {
        _apiClient = apiClient;
        _alertService = alertService;
    }

    public async Task<AuthTokenDto?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var respose = await _apiClient.PostAsync<AuthTokenDto>("/api/v1/auth/login", request, cancellationToken);

        if (respose.IsFailure)
        {
            await _alertService.ShowAsync(new()
            {
                Color = Core.Enums.Colors.Warning,
                Message = respose.ProblemDetails?.Detail ?? "An error occurred while login user.",
            });

            return null;
        }

        return respose.Value;
    }

    public async Task<AuthTokenDto?> RefreshAsync(RefreshRequest request, CancellationToken cancellationToken = default)
    {
        var respose = await _apiClient.PostAsync<AuthTokenDto>("/api/v1/auth/refresh", request, cancellationToken);

        if (respose.IsFailure)
        {
            await _alertService.ShowAsync(new()
            {
                Color = Core.Enums.Colors.Warning,
                Message = respose.ProblemDetails?.Detail ?? "An error occurred while refreshing token.",
            });

            return null;
        }

        return respose.Value;
    }
}