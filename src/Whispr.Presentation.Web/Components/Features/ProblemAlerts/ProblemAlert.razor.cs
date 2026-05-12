using Microsoft.AspNetCore.Components;
using Whispr.Presentation.Web.Components.Ui.Alerts;

namespace Whispr.Presentation.Web.Components.Features.ProblemAlerts;

public partial class ProblemAlert : ComponentBase
{
    [Parameter]
    public ProblemAlertParameters Parameters { get; set; } = new();

    [Parameter]
    public string? CssClass { get; set; }

    private Alert? _alert;

    private bool HasErrors => Parameters.Errors is { Count: > 0 };

    public async Task ShowAsync(ProblemAlertParameters parameters)
    {
        if (_alert is not null)
        {
            await InvokeAsync(() =>
            {
                Parameters = parameters;
                StateHasChanged();
            });

            _alert.Show();
        }
    }

    public async Task HideAsync()
    {
        if (_alert is not null)
        {
            await InvokeAsync(() =>
            {
                Parameters = new ProblemAlertParameters();
                StateHasChanged();
            });

            _alert.Show();
        }
    }
}