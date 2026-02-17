using Whispr.Presentation.Web.Core.Dtos;
using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Components.Features.MessageModals;

public sealed record MessageModalParameters
{
    public Colors HeaderColor { get; set; } = Colors.None;
    public ProblemDetails ProblemDetails { get; set; } = new();
}