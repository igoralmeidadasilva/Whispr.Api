using Microsoft.AspNetCore.Components;
using Whispr.Presentation.Web.Components.Features.ProblemModals;
using Whispr.Presentation.Web.Services.Ui.Modal;

namespace Whispr.Presentation.Web.Layout;

public partial class PublicLayout : LayoutComponentBase, IAsyncDisposable
{
    [Inject]
    public required IModalService ModalService { get; set; }

    private ProblemModal? _modal;

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            ModalService.OnShow += HandleProblemModalShow;
        }
    }

    private async Task HandleProblemModalShow(ProblemModalParameters parameters)
    {
        if (_modal is not null)
        {
            await _modal.ShowAsync(parameters);
        }
    }

    public ValueTask DisposeAsync()
    {
        ModalService.OnShow -= HandleProblemModalShow;
        return ValueTask.CompletedTask;
    }
}