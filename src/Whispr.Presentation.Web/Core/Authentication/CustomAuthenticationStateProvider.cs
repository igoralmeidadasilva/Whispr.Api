using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Whispr.Presentation.Web.Core.Authentication;
using Whispr.Presentation.Web.Core.Dtos;
using Whispr.Presentation.Web.Core.Http;
using Whispr.Presentation.Web.Services.Api.V1.Auth;

public sealed class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IAuthService _authService;
    private readonly ITokenProvider _tokenProvider;
    private readonly ILogger<CustomAuthenticationStateProvider> _logger;

    private readonly ClaimsPrincipal _anonymousUser = new(new ClaimsIdentity());

    public CustomAuthenticationStateProvider(
        IAuthService authService,
        ILogger<CustomAuthenticationStateProvider> logger,
        ITokenProvider tokenProvider)
    {
        _authService = authService;
        _logger = logger;
        _tokenProvider = tokenProvider;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            string? accessToken = _tokenProvider.GetAccessToken();

            if (!string.IsNullOrEmpty(accessToken))
            {
                return new AuthenticationState(BuildClaimsPrincipal(accessToken));
            }

            ApiResponse<AuthTokenDto> response = await _authService.RefreshAsync();

            if (response.IsFailure)
            {
                return new AuthenticationState(_anonymousUser);
            }

            _tokenProvider.SetAccessToken(response.Value!.Token, response.Value!.TokenExpirationAtUtc);

            return new AuthenticationState(BuildClaimsPrincipal(response.Value!.Token));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting authentication state.");
            return new AuthenticationState(_anonymousUser);
        }
    }

    public async ValueTask NotifyUserAuthenticatedAsync(AuthTokenDto token)
    {
        _tokenProvider.SetAccessToken(token.Token, token.TokenExpirationAtUtc);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(BuildClaimsPrincipal(token.Token))));
    }

    public async ValueTask NotifyUserLoggedOutAsync()
    {
        _tokenProvider.Clear();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymousUser)));
    }

    private static ClaimsPrincipal BuildClaimsPrincipal(string token)
    {
        JwtSecurityTokenHandler handler = new();
        JwtSecurityToken jsonToken = handler.ReadJwtToken(token);

        return new(new ClaimsIdentity(jsonToken.Claims, "whisper.api:auth"));
    }
}