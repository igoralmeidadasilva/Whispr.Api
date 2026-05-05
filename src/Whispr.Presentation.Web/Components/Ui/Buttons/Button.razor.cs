using Microsoft.AspNetCore.Components;
using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Components.Ui.Buttons;

public partial class Button : ComponentBase
{
    [Parameter]
    public ButtonType Type { get; set; } = ButtonType.Button;

    [Parameter]
    [EditorRequired]
    public required RenderFragment ChildContent { get; set; }

    [Parameter]
    public ButtonColors Color { get; set; } = ButtonColors.Primary;

    [Parameter]
    public EventCallback OnClick { get; set; }

    [Parameter]
    public bool IsDisabled { get; set; }

    private async Task HandleClick()
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync();
        }
    }
}