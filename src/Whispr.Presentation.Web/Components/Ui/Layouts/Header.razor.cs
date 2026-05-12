using Microsoft.AspNetCore.Components;
using Whispr.Presentation.Web.Core.Enums;
using Whispr.Presentation.Web.Core.Http;
using Whispr.Presentation.Web.Services.Api.V1.Auth;
using Whispr.Presentation.Web.Services.Ui.Modal;

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
    public required IModalService ModalService { get; set; }

    private async Task HandleLogoutOnClick()
    {
        ApiResponse<NoContent> response = await AuthService.LogoutAsync();

        if (response.IsFailure)
        {
            var problemDetails = response.ProblemDetails;

            await ModalService.ShowAsync(new()
            {
                HeaderColor = Colors.Danger,
                CorrelationId = problemDetails!.Extensions?["correlationId"]?.ToString(),
                Title = problemDetails.Title,
                Problem = problemDetails.Detail
            });

            return;
        }

        await AuthenticationStateProvider.NotifyUserLoggedOutAsync();

        NavigationManager.NavigateTo(Routes.Web.Login);
    }
}