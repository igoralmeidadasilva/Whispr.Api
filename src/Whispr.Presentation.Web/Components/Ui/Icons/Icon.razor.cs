using Microsoft.AspNetCore.Components;
using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Components.Ui.Icons;

public partial class Icon : ComponentBase
{
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public IconClasses IconClass { get; set; } = IconClasses.None;

    [Parameter]
    public Colors Color { get; set; } = Colors.None;

    [Parameter]
    public FontSizes FontSize { get; set; } = FontSizes.None;

    [Parameter]
    public string? CssClass { get; set; }
}