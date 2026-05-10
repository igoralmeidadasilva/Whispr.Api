using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Whispr.Presentation.Web;
using Whispr.Presentation.Web.Core.Dtos;
using Whispr.Presentation.Web.Core.Http;
using Whispr.Presentation.Web.Services.Api.V1.Auth;
using Whispr.Presentation.Web.Services.Api.V1.Auth.Requests;

public sealed class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly IAuthService _authService;
    private readonly ILogger<CustomAuthenticationStateProvider> _logger;

    private readonly ClaimsPrincipal _anonymousUser = new(new ClaimsIdentity());

    public CustomAuthenticationStateProvider(ILocalStorageService localStorage, IAuthService authService, ILogger<CustomAuthenticationStateProvider> logger)
    {
        _localStorage = localStorage;
        _authService = authService;
        _logger = logger;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var token = await _localStorage.GetItemAsync<AuthTokenDto>(Constants.LocalStorageKeys.AuthKey);

            if (token is null)
            {
                return new AuthenticationState(_anonymousUser);
            }

            var utcNow = DateTimeOffset.UtcNow;
            var isAccessTokenValid = utcNow < token.AccessTokenExpirationAtUtc;
            var isRefreshTokenValid = utcNow < token.RefreshTokenExpirationAtUtc;

            // First Case: Both Tokens are valid: Authenticate is success
            if (isAccessTokenValid && isRefreshTokenValid)
            {
                return new AuthenticationState(BuildClaimsPrincipal(token));
            }

            // Second Case: Access Token is invalid but Refresh Token is valid: Call refresh endpoint
            if (!isAccessTokenValid && isRefreshTokenValid)
            {
                var request = new RefreshRequest
                {
                    ExpiredAccessToken = token.AccessToken,
                    RefreshToken = token.RefreshToken
                };

                ApiResponse<AuthTokenDto>? response = await _authService.RefreshAsync(request);

                if (response.IsFailure)
                {
                    return new AuthenticationState(_anonymousUser);
                }

                await _localStorage.SetItemAsync(Constants.LocalStorageKeys.AuthKey, response);

                return new AuthenticationState(BuildClaimsPrincipal(response.Value!));
            }

            // Third Case: Both Tokens are invalid: Redirect user to login with return URL and message notification 
            if (!isAccessTokenValid && !isRefreshTokenValid)
            {
                return new AuthenticationState(_anonymousUser);
            }

            // Fourth Case: Access Token is valid but Refresh Token is invalid: Redirect user to login with return URL and message notification
            if (isAccessTokenValid && !isRefreshTokenValid)
            {
                return new AuthenticationState(_anonymousUser);
            }

            return new AuthenticationState(_anonymousUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting authentication state.");
            return new AuthenticationState(_anonymousUser);
        }
    }

    public async ValueTask NotifyUserAuthenticatedAsync(AuthTokenDto token)
    {
        await _localStorage.SetItemAsync(Constants.LocalStorageKeys.AuthKey, token);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(BuildClaimsPrincipal(token))));
    }

    public async ValueTask NotifyUserLoggedOutAsync()
    {
        await _localStorage.RemoveItemAsync(Constants.LocalStorageKeys.AuthKey);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymousUser)));
    }

    private static ClaimsPrincipal BuildClaimsPrincipal(AuthTokenDto token)
    {
        return new(new ClaimsIdentity(
        [
            new(ClaimTypes.Sid, token.UserId.ToString()),
            new(ClaimTypes.Name, token.UserName),
            new(ClaimTypes.Email, token.UserEmail)
        ], "whisper.api:auth"));
    }
}