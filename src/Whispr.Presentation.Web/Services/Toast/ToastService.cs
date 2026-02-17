using Whispr.Presentation.Web.Components.Features.MessageToasts;

namespace Whispr.Presentation.Web.Services.Toast;

public sealed class ToastService : IToastService
{
    public event Func<MessageToastParameters, Task>? OnShow;

    public async Task ShowAsync(MessageToastParameters parameters)
    {
        if (OnShow is not null)
        {
            await OnShow.Invoke(parameters);
        }
    }
}