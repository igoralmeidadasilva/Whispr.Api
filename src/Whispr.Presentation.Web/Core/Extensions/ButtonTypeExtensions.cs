using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Core.Extensions;

public static class ButtonTypeExtensions
{
    public static string ToHtml(this ButtonType buttonType)
    {
        return buttonType switch
        {
            ButtonType.None => string.Empty,
            ButtonType.Button => "button",
            ButtonType.Submit => "submit",
            ButtonType.Reset => "reset",
            _ => string.Empty
        };
    }
}