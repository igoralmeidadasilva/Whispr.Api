using Whispr.Presentation.Web.Components.Features.ProblemModals;
using Whispr.Presentation.Web.Core.Http;

namespace Whispr.Presentation.Web.Services.Ui.Modal;

public sealed class ModalService : IModalService
{
    public event Func<ProblemModalParameters, Task>? OnShow;

    public async Task ShowAsync(ProblemModalParameters parameters)
    {
        if (OnShow is not null)
        {
            await OnShow.Invoke(parameters);
        }
    }
}