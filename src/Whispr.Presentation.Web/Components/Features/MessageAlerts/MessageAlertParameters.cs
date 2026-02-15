using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Components.Features.MessageAlerts;

public sealed record MessageAlertParameters
{
    public string Message { get; set; } = string.Empty;
    public Colors Color { get; set; } = Colors.None;
    public IconClasses IconClass { get; set; } = IconClasses.None;
}