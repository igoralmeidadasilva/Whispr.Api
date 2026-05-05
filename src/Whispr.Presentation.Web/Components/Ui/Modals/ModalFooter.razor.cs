using Microsoft.AspNetCore.Components;
using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Components.Ui.Modals;

public partial class ModalFooter : ComponentBase
{
    [CascadingParameter(Name = "Parent")]
    public Modal? Parent { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string? CssClass { get; set; }

    [Parameter]
    public bool ShowCancelButton { get; set; } = true;

    [Parameter]
    public string CancelLabel { get; set; } = "Cancelar";

    [Parameter]
    public bool ShowConfirmButton { get; set; } = true;

    [Parameter]
    public string ConfirmLabel { get; set; } = "Confirmar";

    [Parameter]
    public EventCallback OnConfirm { get; set; }

    [Parameter]
    public ButtonColors ConfirmColor { get; set; } = ButtonColors.Primary;

    protected override void OnInitialized()
    {
        if (Parent is null)
        {
            throw new InvalidOperationException($"{GetType().Name} must be used within a {nameof(Modal)} component.");
        }
    }

    private async Task HandleClickBtnClose()
    {
        if (Parent is not null)
        {
            await Parent.HideAsync();
        }
    }

    private async Task HandleClickBtnConfirm()
    {
        if (OnConfirm.HasDelegate)
        {
            await OnConfirm.InvokeAsync();
        }
    }
}