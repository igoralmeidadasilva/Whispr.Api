using Microsoft.AspNetCore.Components;
using Whispr.Presentation.Web.Components.Features.MessageAlerts;
using Whispr.Presentation.Web.Core.Enums;
using Whispr.Presentation.Web.Services.Api.V1.Users;
using Whispr.Presentation.Web.Services.Api.V1.Users.Requests;
using Whispr.Presentation.Web.Services.Ui.Alert;
using Whispr.Presentation.Web.Services.Ui.Modal;

namespace Whispr.Presentation.Web.Pages.Public.Register;

public partial class Register : ComponentBase
{
    [Inject]
    public required IAlertService AlertService { get; set; }

    [Inject]
    public required IModalService ModalService { get; set; }

    [Inject]
    public required IUsersService UsersService { get; set; }

    private readonly CreateUserModel _model = new();

    private async Task HandleValidSubmit()
    {
        var request = new CreateUserRequest
        {
            Username = _model.UserName!,
            Email = _model.Email!,
            Password = _model.Password!
        };
        var response = await UsersService.CreateAsync(request);
        if (response is null)
        {
            return;
        }
        var alertParameters = new MessageAlertParameters
        {
            Color = Colors.Success,
            Message = "Your account has been created successfully. You can now log in."
        };
        await AlertService.ShowAsync(alertParameters);
    }
}