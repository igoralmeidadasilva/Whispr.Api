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
    public string? CssClass { get; set; }

    protected override void OnInitialized()
    {
        if (Parent is null)
        {
            throw new InvalidOperationException($"{GetType().Name} must be used within a {nameof(Modal)} component.");
        }
    }
}