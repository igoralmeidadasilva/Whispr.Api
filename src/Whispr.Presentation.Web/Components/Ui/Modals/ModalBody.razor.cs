using Microsoft.AspNetCore.Components;
using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Components.Ui.Modals;

public partial class ModalBody : ComponentBase
{
    [CascadingParameter(Name = "Parent")]
    public Modal? Parent { get; set; }

    [Parameter]
    [EditorRequired]
    public required RenderFragment ChildContent { get; set; }

    [Parameter]
    public Colors Color { get; set; } = Colors.None;

    [Parameter]
    public string? CssClass { get; set; }

    protected override void OnInitialized()
    {
        if (Parent is null)
        {
            throw new InvalidOperationException("ModalHeader must be used within a Modal component.");
        }
    }
}