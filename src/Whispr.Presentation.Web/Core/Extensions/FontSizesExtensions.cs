using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Core.Extensions;

public static class FontSizesExtensions
{
    public static string ToCss(this FontSizes fontSize)
    {
        return fontSize switch
        {
            FontSizes.Fs1 => "fs-1",
            FontSizes.Fs2 => "fs-2",
            FontSizes.Fs3 => "fs-3",
            FontSizes.Fs4 => "fs-4",
            FontSizes.Fs5 => "fs-5",
            FontSizes.Fs6 => "fs-6",
            _ => string.Empty
        };
    }
}