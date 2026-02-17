using Microsoft.AspNetCore.Components;
using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Components.Ui.Buttons;

public partial class Button : ComponentBase
{
    [Parameter]
    public ButtonType Type { get; set; } = ButtonType.Button;

    [Parameter]
    public ButtonColors Color { get; set; } = ButtonColors.Primary;

    [Parameter]
    public string? Label { get; set; }

    [Parameter]
    public IconClasses Icon { get; set; } = IconClasses.None;

    [Parameter]
    public FontSizes FontSize { get; set; } = FontSizes.Fs6;

    [Parameter]
    public EventCallback OnClick { get; set; }

    private async Task HandleClick()
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync();
        }
    }
}