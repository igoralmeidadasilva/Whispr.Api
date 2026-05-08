using Microsoft.AspNetCore.Components;
using Whispr.Presentation.Web.Components.Features.MessageAlerts;
using Whispr.Presentation.Web.Services.Ui.Alert;

namespace Whispr.Presentation.Web.Layout;

public partial class PublicLayout : LayoutComponentBase, IDisposable
{
    [Inject]
    public required IAlertService AlertService { get; set; }

    private MessageAlert? _alert;

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            AlertService.OnShow += HandleMessageAlertShow;
        }
    }

    private async Task HandleMessageAlertShow(MessageAlertParameters parameters)
    {
        if (_alert is not null)
        {
            await _alert.ShowAsync(parameters);
        }
    }

    public void Dispose()
    {
        AlertService.OnShow -= HandleMessageAlertShow;
    }
}