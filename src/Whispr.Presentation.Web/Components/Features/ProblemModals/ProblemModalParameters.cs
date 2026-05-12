using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Components.Features.ProblemModals;

public sealed record ProblemModalParameters
{
    public Colors HeaderColor { get; set; } = Colors.Danger;
    public string? CorrelationId { get; set; }
    public string? Title { get; set; }
    public string? Problem { get; set; }
    public string? Description { get; set; }
    public Dictionary<string, string[]>? Errors { get; set; }
}