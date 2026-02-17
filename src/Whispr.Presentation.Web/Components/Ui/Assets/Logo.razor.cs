using Microsoft.AspNetCore.Components;

namespace Whispr.Presentation.Web.Components.Ui.Assets;

public partial class Logo : ComponentBase
{
    [Parameter]
    public string? CssClass { get; set; }

    private const string ImgPath = "/img/logo.png";
}