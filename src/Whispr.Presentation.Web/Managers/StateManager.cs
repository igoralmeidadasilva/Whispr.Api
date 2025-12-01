using Microsoft.JSInterop;
using System.Text.Json;

namespace Whispr.Presentation.Web.Managers;

public class StateManager<TModel>
{
    private readonly IJSRuntime _jsRuntime;

    public string Key { get; private init; }
    public required TModel Model { get; set; }
    public DateTime LastUpdatedAtUtc { get; private set; }

    public StateManager(IJSRuntime jsRuntime, string key)
    {
        _jsRuntime = jsRuntime;
        Key = key;
        LastUpdatedAtUtc = DateTime.UtcNow;
    }

    public async Task LoadModelAsync()
    {
        var serializedState = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", Key);
        if (string.IsNullOrEmpty(serializedState))
        {
            return;
        }
        Model = JsonSerializer.Deserialize<TModel>(serializedState)
            ?? throw new JsonException($"Failed to deserialize local storage content to {typeof(TModel).Name}.");
    }

    public async Task SaveModelAsync()
    {
        LastUpdatedAtUtc = DateTime.UtcNow;
        var serializedState = JsonSerializer.Serialize(Model);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", Key, serializedState);
    }
}