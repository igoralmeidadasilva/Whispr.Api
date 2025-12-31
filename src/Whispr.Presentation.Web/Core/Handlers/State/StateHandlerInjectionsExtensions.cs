using Microsoft.JSInterop;

namespace Whispr.Presentation.Web.Core.Handlers.State;

public static class StateHandlerInjectionsExtensions
{
    public static IServiceCollection AddStateHandler<TModel>(this IServiceCollection services) where TModel : new()
    {
        services.AddSingleton(sp =>
        {
            IJSRuntime js = sp.GetRequiredService<IJSRuntime>();
            StateHandler<TModel> state = new(js, typeof(TModel).Name)
            {
                Model = new TModel()
            };
            return state;
        });
        return services;
    }

    public static IServiceCollection AddStateHandler<TModel>(
        this IServiceCollection services,
        string localStorageKey)
        where TModel : new()
    {
        services.AddSingleton(sp =>
        {
            IJSRuntime js = sp.GetRequiredService<IJSRuntime>();
            StateHandler<TModel> state = new(js, localStorageKey)
            {
                Model = new TModel()
            };
            return state;
        });
        return services;
    }
}