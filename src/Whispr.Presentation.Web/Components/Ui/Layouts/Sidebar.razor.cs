using Microsoft.AspNetCore.Components;

namespace Whispr.Presentation.Web.Components.Ui.Layouts;

public partial class Sidebar : ComponentBase
{
    private bool isCollapsed = false;

    private void ToggleSidebar()
    {
        isCollapsed = !isCollapsed;
    }
}