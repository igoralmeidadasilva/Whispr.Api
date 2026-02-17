using Microsoft.AspNetCore.Components;
using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Components.Ui.Toasts;

public partial class ToastContainer : ComponentBase
{
    [Parameter]
    public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.End;

    [Parameter]
    public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Bottom;

    [Parameter]
    [EditorRequired]
    public required RenderFragment ChildContent { get; set; }
}