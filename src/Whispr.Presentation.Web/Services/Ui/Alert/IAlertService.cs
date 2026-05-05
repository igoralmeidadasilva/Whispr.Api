using Whispr.Presentation.Web.Components.Features.MessageAlerts;

namespace Whispr.Presentation.Web.Services.Ui.Alert;

public interface IAlertService
{
    event Func<MessageAlertParameters, Task>? OnShow;
    Task ShowAsync(MessageAlertParameters parameters);
}