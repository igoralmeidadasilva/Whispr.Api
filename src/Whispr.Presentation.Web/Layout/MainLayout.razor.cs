using Microsoft.AspNetCore.Components;
using Whispr.Presentation.Web.Components.Features.MessageAlerts;
using Whispr.Presentation.Web.Components.Features.MessageModals;
using Whispr.Presentation.Web.Components.Features.MessageToasts;
using Whispr.Presentation.Web.Components.Ui.Modals;
using Whispr.Presentation.Web.Services.Alert;
using Whispr.Presentation.Web.Services.Modal;
using Whispr.Presentation.Web.Services.Toast;

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
    private MessageModal? _modal;
    private MessageToast? _toast;

    protected override void OnInitialized()
    {
        AlertService.OnShow += HandleMessageAlertShow;
        ModalService.OnShow += HandleMessageModalShow;
        ToastService.OnShow += HandleMessageToastShow;
    }

    private async Task HandleMessageAlertShow(MessageAlertParameters parameters)
    {
        if (_alert is not null)
        {
            await _alert.ShowAsync(parameters);
        }
    }

    private async Task HandleMessageModalShow(MessageModalParameters parameters)
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