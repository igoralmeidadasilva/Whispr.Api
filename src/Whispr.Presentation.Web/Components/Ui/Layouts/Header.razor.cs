using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Whispr.Presentation.Web.Core.Dtos;
using Whispr.Presentation.Web.Core.Http;
using Whispr.Presentation.Web.Services.Api.V1.Auth;
using Whispr.Presentation.Web.Services.Api.V1.Auth.Requests;

namespace Whispr.Presentation.Web.Components.Ui.Layouts;

public partial class Header : ComponentBase
{
    [Parameter]
    public string Id { get; set; } = "headerNavbar";

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    [Inject]
    public required IAuthService AuthService { get; set; }

    [Inject]
    public required CustomAuthenticationStateProvider AuthenticationStateProvider { get; set; }

    [Inject]
    public required ILocalStorageService LocalStorage { get; set; }

    private async Task HandleLogoutOnClick()
    {
        AuthTokenDto? authToken = await LocalStorage.GetItemAsync<AuthTokenDto>(Constants.LocalStorageKeys.AuthKey);

        if (authToken is null)
        {
            return;
        }

        LogoutRequest request = new()
        {
            RefreshToken = authToken.RefreshToken
        };

        ApiResponse<NoContent> response = await AuthService.LogoutAsync(request);

        if (response.IsFailure)
        {
            return;
        }

        await AuthenticationStateProvider.NotifyUserLoggedOutAsync();

        NavigationManager.NavigateTo(Routes.Web.Login);
    }
}