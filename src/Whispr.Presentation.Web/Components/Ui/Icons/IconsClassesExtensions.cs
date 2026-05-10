using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Components.Ui.Icons;

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
            IconClasses.ExclamationTriangleFill => "bi bi-exclamation-triangle-fill",
            IconClasses.ChatDotsFill => "bi bi-chat-dots-fill",
            IconClasses.Eye => "bi bi-eye",
            IconClasses.EyeSlash => "bi bi-eye-slash",
            _ => string.Empty
        };
    }
}