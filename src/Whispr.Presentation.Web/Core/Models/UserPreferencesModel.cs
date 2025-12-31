using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Core.Models;

public sealed record UserPreferencesModel
{
    public Theme Theme { get; set; }
}