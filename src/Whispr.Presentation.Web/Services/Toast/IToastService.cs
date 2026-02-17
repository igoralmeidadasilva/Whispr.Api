using Whispr.Presentation.Web.Components.Features.MessageToasts;

namespace Whispr.Presentation.Web.Services.Toast;

public interface IToastService
{
    event Func<MessageToastParameters, Task>? OnShow;
    Task ShowAsync(MessageToastParameters parameters);
}