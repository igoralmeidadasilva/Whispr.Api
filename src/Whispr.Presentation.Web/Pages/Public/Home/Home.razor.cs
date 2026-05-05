using Microsoft.AspNetCore.Components;
using Whispr.Presentation.Web.Components.Features.MessageAlerts;
using Whispr.Presentation.Web.Components.Features.MessageToasts;
using Whispr.Presentation.Web.Components.Features.ProblemModals;
using Whispr.Presentation.Web.Components.Ui.Modals;
using Whispr.Presentation.Web.Core.Dtos;
using Whispr.Presentation.Web.Core.Enums;
using Whispr.Presentation.Web.Core.Models;
using Whispr.Presentation.Web.Services.Ui.Alert;
using Whispr.Presentation.Web.Services.Ui.Modal;
using Whispr.Presentation.Web.Services.Ui.Toast;

namespace Whispr.Presentation.Web.Pages.Public.Home;

public partial class Home : ComponentBase
{
    [Inject]
    public required IAlertService AlertService { get; set; }

    [Inject]
    public required IModalService ModalService { get; set; }

    [Inject]
    public required IToastService ToastService { get; set; }

    public Modal? _testModalRef;

    private async Task ShowTestModal()
    {
        if (_testModalRef is not null)
        {
            await _testModalRef.ShowAsync();
        }
    }

    private async Task HandleConfirmButton()
    {
        Console.WriteLine("Teste");
    }

    private async Task CreateToastAsync(Colors color)
    {
        var toast = new MessageToastParameters
        {
            Text = DateTime.UtcNow.ToString(),
            Color = color
        };
        await ToastService!.ShowAsync(toast);
    }

    private async Task ShowModalAsync(Colors color)
    {
        //var modal = new ProblemModalParameters
        //{
        //    HeaderColor = color,
        //    ProblemDetails = new ProblemDetails
        //    {
        //        Type = "https://whispr.com/errors/validation-failed",
        //        Title = "Erro de Validação",
        //        Status = 400,
        //        Detail = "Alguns campos enviados são inválidos.",
        //        Instance = "/v1/messages/send",
        //        Extensions = new Dictionary<string, object>
        //        {
        //            { "timestamp", DateTime.UtcNow },
        //            { "code", "WSPR-001" }
        //        },
        //        Errors = new Dictionary<string, string[]>
        //        {
        //            { "Content", new[] { "A mensagem não pode estar vazia." } },
        //            { "RecipientId", new[] { "O destinatário informado não existe." } }
        //        }
        //    }
        //};
        //await ModalService!.ShowAsync(modal);
    }

    private async Task ShowAlertAsync(Colors color)
    {
        var alert = new MessageAlertParameters
        {
            Message = DateTime.UtcNow.ToString(),
            Color = color,
            IconClass = IconClasses.XCircleFill
        };
        await AlertService!.ShowAsync(alert);
    }
}