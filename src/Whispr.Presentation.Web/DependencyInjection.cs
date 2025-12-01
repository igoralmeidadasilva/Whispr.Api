using Microsoft.JSInterop;
using Whispr.Presentation.Web.Managers;

namespace Whispr.Presentation.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddManagerState<TModel>(this IServiceCollection services) where TModel : new()
    {
        services.AddSingleton(sp =>
        {
            var js = sp.GetRequiredService<IJSRuntime>();
            StateManager<TModel> state = new(js, typeof(TModel).Name)
            {
                Model = new TModel()
            };
            return state;
        });
        return services;
    }

    public static IServiceCollection AddManagerState<TModel>(
        this IServiceCollection services,
        string localStorageKey)
        where TModel : new()
    {
        services.AddSingleton(sp =>
        {
            var js = sp.GetRequiredService<IJSRuntime>();
            StateManager<TModel> state = new(js, localStorageKey)
            {
                Model = new TModel()
            };
            return state;
        });
        return services;
    }
}