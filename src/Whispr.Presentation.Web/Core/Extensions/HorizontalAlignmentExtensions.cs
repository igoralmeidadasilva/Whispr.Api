using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Core.Extensions;

public static class HorizontalAlignmentExtensions
{
    public  static string ToCss(this HorizontalAlignment horizontalAlignment)
    {
        return horizontalAlignment switch
        {
            HorizontalAlignment.None => string.Empty,
            HorizontalAlignment.Start => "start-0",
            HorizontalAlignment.Center => "start-50",
            HorizontalAlignment.End => "end-0",
            _ => string.Empty,
        };
    }
}