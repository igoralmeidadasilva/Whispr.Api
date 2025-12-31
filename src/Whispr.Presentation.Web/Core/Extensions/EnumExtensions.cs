using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Core.Extensions;

public static class EnumExtensions
{
    public static string ToDataBsTheme(this Theme theme)
    {
        return theme switch
        {
            Theme.Light => "light",
            Theme.Dark => "dark",
            _ => "light"
        };
    }
}