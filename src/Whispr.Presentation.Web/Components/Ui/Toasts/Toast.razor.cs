using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Components.Ui.Toasts;

public partial class Toast : ComponentBase, IAsyncDisposable
{
    [Parameter]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Parameter]
    public string? Text { get; set; }

    [Parameter]
    public Colors Color { get; set; } = Colors.None;

    [Parameter]
    public TimeSpan Delay { get; set; } = TimeSpan.FromSeconds(10);

    [Parameter]
    public bool ShowAfterRender { get; set; }

    [Inject]
    public required IJSRuntime JSRuntime { get; set; }

    [Parameter]
    public EventCallback<string> OnShowEventCallback { get; set; }

    [Parameter]
    public EventCallback<string> OnShownEventCallback { get; set; }

    [Parameter]
    public EventCallback<string> OnHideEventCallback { get; set; }

    [Parameter]
    public EventCallback<string> OnHiddenEventCallback { get; set; }

    private IJSObjectReference? _jsModule;
    private ElementReference _element;
    private DotNetObjectReference<Toast>? _dotNetRef;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _dotNetRef ??= DotNetObjectReference.Create(this);
            _jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/Ui/Toasts/Toast.razor.js");

            if (ShowAfterRender)
            {
                await ShowAsync();
            }
        }
    }

    public async Task ShowAsync()
    {
        if (_jsModule is not null)
        {
            await _jsModule.InvokeVoidAsync("show", Id, _dotNetRef);
        }
    }

    public async Task HideAsync()
    {
        if (_jsModule is not null)
        {
            await _jsModule.InvokeVoidAsync("hide", Id);
        }
    }

    [JSInvokable]
    public async Task OnShow()
    {
        if (OnShowEventCallback.HasDelegate)
        {
            await OnShowEventCallback.InvokeAsync(Id);
        }
    }

    [JSInvokable]
    public async Task OnShown()
    {
        if (OnShownEventCallback.HasDelegate)
        {
            await OnShownEventCallback.InvokeAsync(Id);
        }
    }

    [JSInvokable]
    public async Task OnHide()
    {
        if (OnHideEventCallback.HasDelegate)
        {
            await OnHideEventCallback.InvokeAsync(Id);
        }
    }

    [JSInvokable]
    public async Task OnHidden()
    {
        if (OnHiddenEventCallback.HasDelegate)
        {
            await OnHiddenEventCallback.InvokeAsync(Id);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_jsModule is not null)
        {
            await _jsModule.InvokeVoidAsync("dispose", Id);
            await _jsModule.DisposeAsync();
        }
    }
}