using Microsoft.AspNetCore.Components;
using Whispr.Presentation.Web.Components.Features.ProblemAlerts;
using Whispr.Presentation.Web.Core.Dtos;
using Whispr.Presentation.Web.Core.Enums;
using Whispr.Presentation.Web.Core.Http;
using Whispr.Presentation.Web.Services.Api.V1.Auth;
using Whispr.Presentation.Web.Services.Api.V1.Auth.Requests;
using Whispr.Presentation.Web.Services.Api.V1.Users;
using Whispr.Presentation.Web.Services.Api.V1.Users.Requests;
using Whispr.Presentation.Web.Services.Ui.Modal;

namespace Whispr.Presentation.Web.Pages.Public.Login;

public partial class Login : ComponentBase
{

    [SupplyParameterFromQuery]
    public string? ReturnUrl { get; set; }

    [Inject]
    public required CustomAuthenticationStateProvider AuthenticationStateProvider { get; set; }

    [Inject]
    public required IAuthService AuthService { get; set; }

    [Inject]
    public required IUsersService UsersService { get; set; }

    [Inject]
    public required ILogger<Login> Logger { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    [Inject]
    public required IModalService ModalService { get; set; }

    private LoginModel _loginModel = new();

    private ProblemAlert? _alert;

    private async Task HandleLogin()
    {
        try
        {
            var request = new LoginRequest
            {
                Email = _loginModel.Email!,
                Password = _loginModel.Password!
            };

            ApiResponse<AuthTokenDto>? response = await AuthService.LoginAsync(request);

            if (response.IsFailure)
            {
                Logger.LogError("Error occurred while logging in.");

                var problemDetails = response.ProblemDetails;

                if (problemDetails!.Status >= 500)
                {
                    await ModalService.ShowAsync(new()
                    {
                        HeaderColor = Colors.Danger,
                        CorrelationId = problemDetails.Extensions?["correlationId"]?.ToString(),
                        Title = problemDetails.Title,
                        Problem = problemDetails.Detail
                    });

                    return;
                }

                await _alert!.ShowAsync(new()
                {
                    Problem = problemDetails.Detail,
                    Errors = problemDetails.Errors
                });

                return;
            }

            await AuthenticationStateProvider!.NotifyUserAuthenticatedAsync(response.Value!);

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

            ApiResponse<AuthTokenDto>? response = await AuthService.LoginWithGoogleAsync(request);

            if (response.IsFailure)
            {
                Logger.LogError("Error occurred while logging in.");

                var problemDetails = response.ProblemDetails;

                if (problemDetails!.Status >= 500)
                {
                    await ModalService.ShowAsync(new()
                    {
                        HeaderColor = Colors.Danger,
                        CorrelationId = problemDetails.Extensions?["correlationId"]?.ToString(),
                        Title = problemDetails.Title,
                        Problem = problemDetails.Detail
                    });

                    return;
                }

                await _alert!.ShowAsync(new()
                {
                    Problem = problemDetails.Detail,
                    Errors = problemDetails.Errors
                });

                return;
            }

            await AuthenticationStateProvider!.NotifyUserAuthenticatedAsync(response.Value!);

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

        NavigationManager.NavigateTo(Routes.Web.Home);
    }
}