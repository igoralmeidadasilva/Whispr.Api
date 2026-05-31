using Microsoft.AspNetCore.Components;
using Whispr.Presentation.Web.Components.Features.ProblemAlerts;
using Whispr.Presentation.Web.Core.Enums;
using Whispr.Presentation.Web.Core.Http;
using Whispr.Presentation.Web.Services.Api.V1.Users;
using Whispr.Presentation.Web.Services.Api.V1.Users.Requests;
using Whispr.Presentation.Web.Services.Ui.Modal;

namespace Whispr.Presentation.Web.Pages.Public.Register;

public partial class Register : ComponentBase
{
    [Inject]
    public required IUsersService UsersService { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    [Inject]
    public required IModalService ModalService { get; set; }

    private readonly CreateUserModel _model = new();
    private ProblemAlert? _alert;
    private bool _isLoading = false;

    private async Task HandleValidSubmit()
    {
        try
        {
            _isLoading = true;

            CreateUserRequest request = new()
            {
                Username = _model.UserName!,
                Email = _model.Email!,
                Password = _model.Password!,
                ConfirmPassword = _model.ConfirmPassword!
            };
            ApiResponse<NoContent> response = await UsersService.CreateAsync(request);

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
            _isLoading = false;
        }
    }
}