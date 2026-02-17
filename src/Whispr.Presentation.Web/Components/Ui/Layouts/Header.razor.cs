using Microsoft.AspNetCore.Components;

namespace Whispr.Presentation.Web.Components.Ui.Layouts;

public partial class Header : ComponentBase
{
    [Parameter] public string Id { get; set; } = "headerNavbar";
}