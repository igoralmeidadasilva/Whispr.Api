namespace Whispr.Presentation.Web.Core.Extensions;

public static class ButtonColorsExtensions
{
    public static string ToCss(this Enums.ButtonColors buttonColors)
    {
        return buttonColors switch
        {
            Enums.ButtonColors.None => string.Empty,
            Enums.ButtonColors.Primary => "btn-primary",
            Enums.ButtonColors.OutlinePrimary => "btn-outline-primary",
            Enums.ButtonColors.Secondary => "btn-secondary",
            Enums.ButtonColors.OutlineSecondary => "btn-outline-secondary",
            Enums.ButtonColors.Success => "btn-success",
            Enums.ButtonColors.OutlineSuccess => "btn-outline-success",
            Enums.ButtonColors.Danger => "btn-danger",
            Enums.ButtonColors.OutlineDanger => "btn-outline-danger",
            Enums.ButtonColors.Warning => "btn-warning",
            Enums.ButtonColors.OutlineWarning => "btn-outline-warning",
            Enums.ButtonColors.Info => "btn-info",
            Enums.ButtonColors.OutlineInfo => "btn-outline-info",
            Enums.ButtonColors.Light => "btn-light",
            Enums.ButtonColors.OutlineLight => "btn-outline-light",
            Enums.ButtonColors.Dark => "btn-dark",
            Enums.ButtonColors.OutlineDark => "btn-outline-dark",
            _ => string.Empty
        };
    }
}