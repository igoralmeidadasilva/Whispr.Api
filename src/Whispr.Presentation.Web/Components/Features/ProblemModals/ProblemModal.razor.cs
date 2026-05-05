using Microsoft.AspNetCore.Components;
using Whispr.Presentation.Web.Components.Ui.Modals;
using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Components.Features.ProblemModals;

public partial class ProblemModal : ComponentBase
{
    [Parameter]
    public ProblemModalParameters Parameters { get; set; } = new();

    private Modal? _modal;

    public async Task ShowAsync(ProblemModalParameters parameters)
    {
        if (_modal is not null)
        {
            await InvokeAsync(() =>
            {
                Parameters = parameters;
                StateHasChanged();
            });

            await _modal.ShowAsync();
        }
    }

    public async Task HideAsync()
    {
        if (_modal is not null)
        {
            await InvokeAsync(() =>
            {
                Parameters = new ProblemModalParameters();
                StateHasChanged();
            });

            await _modal.HideAsync();
        }
    }
}