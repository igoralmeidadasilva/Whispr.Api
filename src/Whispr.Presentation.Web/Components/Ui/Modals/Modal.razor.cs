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

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _dotNetRef ??= DotNetObjectReference.Create(this);
            _module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/Ui/Modals/Modal.razor.js");
            await _module.InvokeVoidAsync("initialize", _element, _dotNetRef);
        }
    }

    public async Task ShowAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("show", _element);
        }
    }

    public async Task HideAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("hide", _element);
        }
    }

    /// <summary>
    /// Triggered by the Bootstrap 'show.bs.modal' event.
    /// Invokes the <see cref="OnShow"/> callback before the modal is displayed.
    /// </summary>
    [JSInvokable]
    public async Task HandleShow()
    {
        if (OnShow.HasDelegate)
        {
            await OnShow.InvokeAsync();
        }
    }

    /// <summary>
    /// Triggered by the Bootstrap 'shown.bs.modal' event.
    /// Invokes the <see cref="OnShown"/> callback after the modal becomes visible to the user.
    /// </summary>
    [JSInvokable]
    public async Task HandleShown()
    {
        if (OnShown.HasDelegate)
        {
            await OnShown.InvokeAsync();
        }
    }

    /// <summary>
    /// Triggered by the Bootstrap 'hide.bs.modal' event.
    /// Invokes the <see cref="OnHide"/> callback immediately after the hide method is called.
    /// </summary>
    [JSInvokable]
    public async Task HandleHide()
    {
        if (OnHide.HasDelegate)
        {
            await OnHide.InvokeAsync();
        }
    }

    /// <summary>
    /// Triggered by the Bootstrap 'hidden.bs.modal' event.
    /// Invokes the <see cref="OnHidden"/> callback when the modal has finished being hidden (after CSS transitions complete).
    /// </summary>
    [JSInvokable]
    public async Task HandleHidden()
    {
        if (OnHidden.HasDelegate)
        {
            await OnHidden.InvokeAsync();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("dispose", _element);
            await _module.DisposeAsync();
        }
        _dotNetRef?.Dispose();
    }
}