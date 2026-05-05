using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Components.Features.MessageAlerts;

public sealed record MessageAlertParameters
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public Colors Color { get; set; } = Colors.None;
    public IconClasses IconClass { get; set; } = IconClasses.None;
    public string? Message { get; set; }
}