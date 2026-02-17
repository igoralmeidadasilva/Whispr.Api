using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Core.Extensions;

public static class VerticalAlignmentExtensions
{
    public static string ToCss(this VerticalAlignment verticalAlignment)
    {
        return verticalAlignment switch
        {
            VerticalAlignment.None => string.Empty,
            VerticalAlignment.Top => "top-0",
            VerticalAlignment.Bottom => "bottom-0",
            _ => string.Empty,
        };
    }
}