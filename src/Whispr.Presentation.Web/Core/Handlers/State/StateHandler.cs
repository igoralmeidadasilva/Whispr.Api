using Microsoft.JSInterop;
using System.Text.Json;

namespace Whispr.Presentation.Web.Core.Handlers.State;

public class StateHandler<TModel>
{
    private readonly IJSRuntime _jsRuntime;
    public string Key { get; private init; }
    public required TModel Model { get; set; }
    public DateTime LastUpdatedAtUtc { get; private set; }

    public StateHandler(IJSRuntime jsRuntime, string key)
    {
        _jsRuntime = jsRuntime;
        Key = key;
        LastUpdatedAtUtc = DateTime.UtcNow;
    }

    public async Task LoadModelAsync(CancellationToken cancellationToken = default)
    {
        string serializedState = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", cancellationToken, Key);
        if (string.IsNullOrEmpty(serializedState))
        {
            return;
        }
        Model = JsonSerializer.Deserialize<TModel>(serializedState)
            ?? throw new JsonException($"Failed to deserialize local storage content to {typeof(TModel).Name}.");
    }

    public async Task SaveModelAsync(CancellationToken cancellationToken = default)
    {
        LastUpdatedAtUtc = DateTime.UtcNow;
        var serializedState = JsonSerializer.Serialize(Model);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", cancellationToken, Key, serializedState);
    }
}