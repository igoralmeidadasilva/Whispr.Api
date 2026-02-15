using Microsoft.AspNetCore.Components;
using Whispr.Presentation.Web.Components.Features.MessageAlerts;
using Whispr.Presentation.Web.Components.Features.MessageModals;
using Whispr.Presentation.Web.Components.Ui.Modals;
using Whispr.Presentation.Web.Core.Dtos;
using Whispr.Presentation.Web.Services.Alert;
using Whispr.Presentation.Web.Services.Modal;

namespace Whispr.Presentation.Web.Pages.Public.Home;

public partial class Home : ComponentBase
{
    [Inject]
    public required IAlertService AlertService { get; set; }

    [Inject]
    public required IModalService ModalService { get; set; }

    private async Task ShowModalAsync()
    {
        var parameters = new MessageModalParameters
        {
            ProblemDetails = new ProblemDetails
            {
                Title = "An error occurred",
                Detail = "Something went wrong while processing your request.",
                Extensions = new Dictionary<string, object>
                {
                    { "correlationId", Guid.NewGuid().ToString() }
                },
                Errors = new Dictionary<string, string[]>
                {
                    { "field1", new[] { "Error message for field1" } },
                    { "field2", new[] { "Error message for field2" } }
                }
            },
            HeaderColor = Core.Enums.Colors.Danger
        };
        await ModalService.ShowAsync(parameters);
    }
}