using Microsoft.AspNetCore.Components;
using System.Text;
using Whispr.Presentation.Web.Core.Enums;
using Whispr.Presentation.Web.Core.Extensions;

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
    public Rounded Rounded { get; set; } = Rounded.None;

    [Parameter]
    public EventCallback OnClick { get; set; }

    [Parameter]
    public string? CssClass { get; set; }

    [Parameter]
    public bool IsDisabled { get; set; }

    [Parameter]
    public bool IsLoading { get; set; }

    private bool IsLoadingOrDisabled => IsLoading || IsDisabled;

    private async Task HandleClick()
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync();
        }
    }

    private string BuildCssClass()
    {
        StringBuilder css =  new($"btn {Color.ToCss()}");

        if (Rounded != Rounded.None)
        {
            css.Append($" {Rounded.ToCss()}");
        }

        if (!string.IsNullOrEmpty(CssClass))
        {
            css.Append($" {CssClass}");
        }

        return css.ToString();
    }
}