using Microsoft.AspNetCore.Components;
using Whispr.Presentation.Web.Core.Dtos;
using Whispr.Presentation.Web.Services.Api.V1.Auth;
using Whispr.Presentation.Web.Services.Api.V1.Auth.Requests;

namespace Whispr.Presentation.Web.Pages.Public.Login;

public partial class Login : ComponentBase
{
    [Inject]
    public required CustomAuthenticationStateProvider AuthenticationStateProvider { get; set; }

    [SupplyParameterFromQuery]
    public string? ReturnUrl { get; set; }

    [Inject]
    public required IAuthService AuthService { get; set; }

    [Inject]
    public required ILogger<Login> Logger { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    private string Email { get; set; } = string.Empty;
    private string Password { get; set; } = string.Empty;

    private async Task HandleLogin()
    {
        try
        {
            var request = new LoginRequest
            {
                Email = Email,
                Password = Password
            };
            AuthTokenDto? response = await AuthService.LoginAsync(request);

            if (response is null)
            {
                Logger.LogError("Error occurred while logging in.");
                return;
            }

            await AuthenticationStateProvider!.NotifyUserAuthenticatedAsync(response!);

            RedirectAfterLogin();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error occurred while logging in.");
        }
    }

    private async Task HandleGoogleLogin(string idToken)
    {
        try
        {
            LoginWithGoogleRequest request = new()
            {
                IdToken = idToken
            };

            AuthTokenDto? response = await AuthService.LoginWithGoogleAsync(request);

            if (response is null)
            {
                Logger.LogError("Error occurred while logging in.");
                return;
            }

            await AuthenticationStateProvider!.NotifyUserAuthenticatedAsync(response!);

            RedirectAfterLogin();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error occurred while logging in.");
        }
    }

    private void RedirectAfterLogin()
    {
        if (!string.IsNullOrEmpty(ReturnUrl))
        {
            NavigationManager.NavigateTo(ReturnUrl);
            return;
        }

        NavigationManager.NavigateTo("/");
    }
}