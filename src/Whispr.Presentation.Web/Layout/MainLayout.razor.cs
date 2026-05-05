using Microsoft.AspNetCore.Components;
using Whispr.Presentation.Web.Components.Features.MessageAlerts;
using Whispr.Presentation.Web.Components.Features.MessageToasts;
using Whispr.Presentation.Web.Components.Features.ProblemModals;
using Whispr.Presentation.Web.Services.Ui.Alert;
using Whispr.Presentation.Web.Services.Ui.Modal;
using Whispr.Presentation.Web.Services.Ui.Toast;

namespace Whispr.Presentation.Web.Layout;

public partial class MainLayout : LayoutComponentBase, IDisposable
{
    [Inject]
    public required IAlertService AlertService { get; set; }

    [Inject]
    public required IModalService ModalService { get; set; }

    [Inject]
    public required IToastService ToastService { get; set; }

    private MessageAlert? _alert;
    private ProblemModal? _modal;
    private MessageToast? _toast;

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            AlertService.OnShow += HandleMessageAlertShow;
            ModalService.OnShow += HandleMessageModalShow;
            ToastService.OnShow += HandleMessageToastShow;
        }
    }

    private async Task HandleMessageAlertShow(MessageAlertParameters parameters)
    {
        if (_alert is not null)
        {
            await _alert.ShowAsync(parameters);
        }
    }

    private async Task HandleMessageModalShow(ProblemModalParameters parameters)
    {
        if (_modal is not null)
        {
            await _modal.ShowAsync(parameters);
        }
    }

    private async Task HandleMessageToastShow(MessageToastParameters parameters)
    {
        if (_toast is not null)
        {
            await _toast.CreateAsync(parameters);
        }
    }

    public void Dispose()
    {
        AlertService.OnShow -= HandleMessageAlertShow;
        ModalService.OnShow -= HandleMessageModalShow;
        ToastService.OnShow -= HandleMessageToastShow;
    }
}