using Whispr.Presentation.Web.Components.Features.MessageModals;

namespace Whispr.Presentation.Web.Services.Modal;

public sealed class ModalService : IModalService
{
    public event Func<MessageModalParameters, Task>? OnShow;

    public async Task ShowAsync(MessageModalParameters parameters)
    {
        if (OnShow is not null)
        {
            await OnShow.Invoke(parameters);
        }
    }
}