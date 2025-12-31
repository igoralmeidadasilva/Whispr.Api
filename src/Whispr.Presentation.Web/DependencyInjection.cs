using Whispr.Presentation.Web.Core.Handlers.State;
using Whispr.Presentation.Web.Core.Models;

namespace Whispr.Presentation.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddManagers(this IServiceCollection services)
    {
        services.AddStateHandler<UserPreferencesModel>("user-preferences");
        return services;
    }
}