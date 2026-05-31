using Microsoft.AspNetCore.Components;

namespace Whispr.Presentation.Web.Components.Features.CardContainer;

public partial class CardContainer : ComponentBase
{
    [Parameter]
    public required string Title { get; set; }
    [Parameter]
    public required RenderFragment ChildContent { get; set; }
}