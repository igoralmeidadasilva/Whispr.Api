using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Core.Extensions;

public static class IconsClassesExtensions
{
    public static string ToCss(this IconClasses iconClass)
    {
        return iconClass switch
        {
            IconClasses.None => string.Empty,
            IconClasses.InfoCircleFill => "bi bi-info-circle-fill",
            IconClasses.CheckCircleFill => "bi bi-check-circle-fill",
            IconClasses.ExclamationCircleFill => "bi-exclamation-circle-fill",
            IconClasses.XCircleFill => "bi bi-x-circle-fill",
            _ => string.Empty
        };
    }
}