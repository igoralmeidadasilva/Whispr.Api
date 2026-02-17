using Whispr.Presentation.Web.Components.Features.MessageAlerts;
using Whispr.Presentation.Web.Components.Features.MessageModals;

namespace Whispr.Presentation.Web.Services.Modal;

public interface IModalService
{
    event Func<MessageModalParameters, Task>? OnShow;
    Task ShowAsync(MessageModalParameters parameters);
}