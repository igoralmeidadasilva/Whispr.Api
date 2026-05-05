using Microsoft.AspNetCore.Components;
using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Components.Ui.Modals;

public partial class ModalHeader : ComponentBase
{
    [CascadingParameter(Name = "Parent")]
    public Modal? Parent { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    public Colors Color { get; set; } = Colors.None;

    [Parameter]
    public string? CssClass { get; set; }

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
}