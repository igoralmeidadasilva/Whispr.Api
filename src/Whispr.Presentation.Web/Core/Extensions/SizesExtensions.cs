using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Core.Extensions;

public static class SizesExtensions
{
    public static string ToModalCss(this Sizes size)
    {
        return size switch
        {
            Sizes.None => string.Empty,
            Sizes.Sm => "modal-sm",
            Sizes.Lg => "modal-lg",
            Sizes.Xl => "modal-xl",
            _ => string.Empty
        };
    }
}