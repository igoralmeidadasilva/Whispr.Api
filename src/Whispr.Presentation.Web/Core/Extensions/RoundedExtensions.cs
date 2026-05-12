namespace Whispr.Presentation.Web.Core.Extensions;

public static class RoundedExtensions
{
    public static string ToCss(this Enums.Rounded rounded)
    {
        return rounded switch
        {
            Enums.Rounded.None => string.Empty,
            Enums.Rounded.One => "rounded-1",
            Enums.Rounded.Two => "rounded-2",
            Enums.Rounded.Three => "rounded-3",
            Enums.Rounded.Four => "rounded-4",
            Enums.Rounded.Five => "rounded-5",
            _ => string.Empty
        };
    }
}