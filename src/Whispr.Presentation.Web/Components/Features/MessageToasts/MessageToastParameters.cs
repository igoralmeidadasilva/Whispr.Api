using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Components.Features.MessageToasts;

public sealed record MessageToastParameters
{
    private const int DefaultDelaySeconds = 5;
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public string? Text { get; set; }
    public Colors Color { get; set; }
    public TimeSpan Delay { get; set; } = TimeSpan.FromSeconds(DefaultDelaySeconds);
}