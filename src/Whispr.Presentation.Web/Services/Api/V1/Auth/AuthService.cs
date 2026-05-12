using Whispr.Presentation.Web.Core.Dtos;
using Whispr.Presentation.Web.Core.Http;
using Whispr.Presentation.Web.Services.Api.V1.Auth.Requests;

namespace Whispr.Presentation.Web.Services.Api.V1.Auth;

public sealed class AuthService : IAuthService
{
    private readonly IApiClient _apiClient;

    public AuthService(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<ApiResponse<AuthTokenDto>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        return await _apiClient.PostAsync<AuthTokenDto>(Routes.Api.Auth.Login, request, cancellationToken);
    }

    public async Task<ApiResponse<AuthTokenDto>> LoginWithGoogleAsync(LoginWithGoogleRequest request, CancellationToken cancellationToken = default)
    {
        return await _apiClient.PostAsync<AuthTokenDto>(Routes.Api.Auth.LoginWithGoogle, request, cancellationToken);
    }

    public async Task<ApiResponse<AuthTokenDto>> RefreshAsync(CancellationToken cancellationToken = default)
    {
        return await _apiClient.PostAsync<AuthTokenDto>(Routes.Api.Auth.Refresh, null!, cancellationToken);
    }

    public async Task<ApiResponse<NoContent>> LogoutAsync(CancellationToken cancellationToken = default)
    {
        return await _apiClient.PostAsync(Routes.Api.Auth.Logout, null!, cancellationToken);
    }
}