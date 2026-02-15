using Microsoft.AspNetCore.Components;
using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Components.Ui.Modals;

public partial class ModalFooter : ComponentBase
{
    [CascadingParameter(Name = "Parent")]
    public Modal? Parent { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public Colors Color { get; set; } = Colors.None;

    [Parameter]
    public bool ShowCloseButton { get; set; } = true;

    [Parameter]
    public string CloseButtonLabel { get; set; } = "Fechar";
    [Parameter]
    public Colors CloseButtonColor { get; set; } = Colors.Secondary;

    [Parameter]
    public bool ShowActionButton { get; set; } = true;

    [Parameter]
    public string ActionButtonLabel { get; set; } = "Confirmar";

    [Parameter]
    public Colors ActionButtonColor { get; set; } = Colors.Primary;

    [Parameter]
    public EventCallback OnClickActionButton { get; set; }

    protected override void OnInitialized()
    {
        if (Parent is null)
        {
            throw new InvalidOperationException("ModalFooter must be used within a Modal component.");
        }
    }

    private async Task HandleClickBtnAction()
    {
        if (OnClickActionButton.HasDelegate)
        {
            await OnClickActionButton.InvokeAsync();
        }
    }

    private async Task HandleClickBtnClose()
    {
        if (Parent is not null)
        {
            await Parent.HideAsync();
        }
    }
}