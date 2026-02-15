using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Whispr.Presentation.Web.Components.Ui.Modals;

public partial class Modal : ComponentBase, IAsyncDisposable
{
    [Parameter]
    [EditorRequired]
    public required RenderFragment ChildContent { get; set; }

    [Parameter]
    public required string Id { get; set; } = Guid.NewGuid().ToString();

    [Parameter]
    public bool IsStaticBackdrop { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    [Inject]
    public required IJSRuntime JSRuntime { get; set; }

    private ElementReference? _element;
    private IJSObjectReference? _module;

    protected override void OnInitialized()
    {
        LoadStaticBackdrop();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/Ui/Modals/Modal.razor.js");
            await _module.InvokeVoidAsync("initialize", _element);
        }
    }

    private void LoadStaticBackdrop()
    {
        AdditionalAttributes ??= [];
        if (IsStaticBackdrop)
        {
            AdditionalAttributes.Add("data-bs-backdrop", "static");
            AdditionalAttributes.Add("data-bs-keyboard", "false");
        }
    }

    public async Task ShowAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("show");
        }
    }

    public async Task HideAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("hide");
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
        {
            await _module.DisposeAsync();
        }
    }
}