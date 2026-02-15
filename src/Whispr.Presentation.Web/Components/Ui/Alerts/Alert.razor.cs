using Microsoft.AspNetCore.Components;
using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Components.Ui.Alerts;

public partial class Alert : ComponentBase
{
    [Parameter]
    public required string Id { get; set; } = Guid.NewGuid().ToString();

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public bool IsShow { get; set; } = false;

    [Parameter]
    public bool IsDismissable { get; set; } = true;

    [Parameter]
    public Colors Color { get; set; } = Colors.None;

    [Parameter]
    public string? CssClass { get; set; }

    public ElementReference Element { get; set; }

    public void Show()
    {
        IsShow = true;
        StateHasChanged();
    }

    public void Hide()
    {
        IsShow = false;
        StateHasChanged();
    }

    public void Toggle()
    {
        IsShow = !IsShow;
        StateHasChanged();
    }
}