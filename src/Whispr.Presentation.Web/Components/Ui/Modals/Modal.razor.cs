using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Components.Ui.Modals;

public partial class Modal : ComponentBase, IAsyncDisposable
{
    [Parameter]
    [EditorRequired]
    public required RenderFragment ChildContent { get; set; }

    [Parameter]
    public required string Id { get; set; } = Guid.NewGuid().ToString();

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public string? CssClass { get; set; }

    [Parameter]
    public Sizes Size { get; set; } = Sizes.None;

    [Parameter]
    public EventCallback OnShow { get; set; }

    [Parameter]
    public EventCallback OnShown { get; set; }

    [Parameter]
    public EventCallback OnHide { get; set; }

    [Parameter]
    public EventCallback OnHidden { get; set; }

    [Parameter]
    public bool IsStatic { get; set; } = false;

    [Inject]
    public required IJSRuntime JSRuntime { get; set; }

    private DotNetObjectReference<Modal>? _dotNetRef;
    private ElementReference? _element;
    private IJSObjectReference? _module;
    private bool _disposed;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _dotNetRef ??= DotNetObjectReference.Create(this);
            _module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/Ui/Modals/Modal.razor.js");

            if (_disposed) { 
                return;
            }
            await _module.InvokeVoidAsync("initialize", _element, _dotNetRef);
        }
    }

    public async Task ShowAsync()
    {
        if (_disposed || _module is null)
        {
            return;
        }
        await _module.InvokeVoidAsync("show", _element);
    }

    public async Task HideAsync()
    {
        if (_disposed || _module is null)
        {
            return;
        }
        await _module.InvokeVoidAsync("hide", _element);
    }

    [JSInvokable]
    public async Task HandleShow()
    {
        if (_disposed)
        {
            return;
        }
        if (OnShow.HasDelegate)
        {
            await OnShow.InvokeAsync();
        }
    }

    [JSInvokable]
    public async Task HandleShown()
    {
        if (_disposed)
        {
            return;
        }
        if (OnShown.HasDelegate)
        {
            await OnShown.InvokeAsync();
        }
    }

    [JSInvokable]
    public async Task HandleHide()
    {
        if (_disposed)
        {
            return;
        }
        if (OnHide.HasDelegate)
        {
            await OnHide.InvokeAsync();
        }
    }

    [JSInvokable]
    public async Task HandleHidden()
    {
        if (_disposed)
        {
            return;
        }
        if (OnHidden.HasDelegate)
        {
            await OnHidden.InvokeAsync();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }
        _disposed = true;

        if (_module is not null)
        {
            try
            {
                await _module.InvokeVoidAsync("dispose", _element);
            }
            catch (JSDisconnectedException) { }
            catch (TaskCanceledException) { }
            finally
            {
                await _module.DisposeAsync();
            }
        }

        _dotNetRef?.Dispose();
    }
}