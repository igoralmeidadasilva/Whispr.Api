using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Core.Extensions;

public static class ThemesExtensions
{
    public static string ToDataBsTheme(this Themes theme)
    {
        return theme switch
        {
            Themes.Light => "light",
            Themes.Dark => "dark",
            _ => "light"
        };
    }
}