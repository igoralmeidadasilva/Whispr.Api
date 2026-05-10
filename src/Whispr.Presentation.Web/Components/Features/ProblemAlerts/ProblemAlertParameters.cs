using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Components.Features.ProblemAlerts;

public sealed record ProblemAlertParameters
{
    public Colors HeaderColor { get; set; } = Colors.Warning;
    public string? Problem { get; set; }
    public Dictionary<string, string[]>? Errors { get; set; }
}