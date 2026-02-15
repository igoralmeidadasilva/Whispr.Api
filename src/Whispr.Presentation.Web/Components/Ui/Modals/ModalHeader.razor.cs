using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Components.Ui.Modals;

public partial class ModalHeader : ComponentBase
{
    [CascadingParameter(Name = "Parent")]
    public Modal? Parent { get; set; }

    [Parameter]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    public Colors Color { get; set; } = Colors.None;

    [Parameter]
    public bool ShowCloseButton { get; set; } = true;

    [Parameter]
    public string? CssClass { get; set; }

    protected override void OnInitialized()
    {
        if (Parent is null)
        {
            throw new InvalidOperationException("ModalHeader must be used within a Modal component.");
        }
    }

    private async Task HandleClickBtnClose()
    {
        if (Parent is not null)
        {
            await Parent.HideAsync();
        }
    }

    private string GetTextColor()
    {

        if (Color is Colors.None or Colors.Light)
        {
            return "text-dark";
        }
        return "text-white";
    }

    private string GetBtnCloseColor()
    {
        if (Color is Colors.None or Colors.Light)
        {
            return string.Empty;
        }
        return "btn-close-white";
    }
}