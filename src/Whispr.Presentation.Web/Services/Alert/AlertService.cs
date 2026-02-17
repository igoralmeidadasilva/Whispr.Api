using Whispr.Presentation.Web.Components.Features.MessageAlerts;

namespace Whispr.Presentation.Web.Services.Alert;

public sealed class AlertService : IAlertService
{
    public event Func<MessageAlertParameters, Task>? OnShow;

    public async Task ShowAsync(MessageAlertParameters parameters)
    {
        if (OnShow is not null)
        {
            await OnShow.Invoke(parameters);
        }
    }
}