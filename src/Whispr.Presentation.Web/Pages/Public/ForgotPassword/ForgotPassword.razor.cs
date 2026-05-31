using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Whispr.Presentation.Web.Components.Features.ProblemAlerts;
using Whispr.Presentation.Web.Core.Enums;
using Whispr.Presentation.Web.Core.Http;
using Whispr.Presentation.Web.Services.Api.V1.Users;
using Whispr.Presentation.Web.Services.Api.V1.Users.Requests;
using Whispr.Presentation.Web.Services.Ui.Modal;

namespace Whispr.Presentation.Web.Pages.Public.ForgotPassword;

public partial class ForgotPassword : ComponentBase, IAsyncDisposable
{
    [Inject]
    public required IUsersService UsersService { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    [Inject]
    public required IModalService ModalService { get; set; }

    [Inject]
    public required IJSRuntime JSRuntime { get; set; }

    private readonly CreatePasswordRecoveryCodeModel _passwordRecoveryCodeModel = new();
    private readonly ChangePasswordModel _passwordResetModel = new();

    private ProblemAlert? _alert;
    private IJSObjectReference? _module;

    private bool _hasRecoveryCode;
    private bool _shouldConfigureDigits;

    private bool _isLoadingPasswordRecoveryCode;
    private bool _isLoadingPasswordReset;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Pages/Public/ForgotPassword/ForgotPassword.razor.js");
        }

        if (_shouldConfigureDigits && _module is not null)
        {
            _shouldConfigureDigits = false;
            await _module.InvokeVoidAsync("configureDigitInputs");
        }
    }

    private async Task HandleSubmitCreatePasswordRecoveryCode()
    {
        try
        {
            _isLoadingPasswordRecoveryCode = true;

            CreatePasswordRecoveryCodeRequest request = new()
            {
                Email = _passwordRecoveryCodeModel.Email!
            };

            ApiResponse<NoContent> response = await UsersService.CreatePasswordRecoveryCode(request);

            if (response.IsFailure)
            {
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
            _passwordResetModel.Email = request.Email;
            _hasRecoveryCode = true;
            _shouldConfigureDigits = true;
        }
        finally
        {
            _isLoadingPasswordRecoveryCode = false;
        }
    }

    private async Task HandleSubmitChangePassword()
    {
        try
        {
            _isLoadingPasswordReset = true;

            ChangePasswordRequest request = new()
            {
                Email = _passwordResetModel.Email!,
                RecoveryCode = _passwordResetModel.RecoveryCode!,
                NewPassword = _passwordResetModel.NewPassword!,
                ConfirmNewPassword = _passwordResetModel.ConfirmNewPassword!
            };

            ApiResponse<NoContent> response = await UsersService.ChangePassword(request);

            if (response.IsFailure)
            {
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

            NavigationManager.NavigateTo(Routes.Web.Login);
        }
        finally
        {
            _isLoadingPasswordReset = false;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
        {
            await _module.DisposeAsync();
        }
    }
}