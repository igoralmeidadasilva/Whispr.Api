using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Whispr.Presentation.Web.Core.Options;

namespace Whispr.Presentation.Web.Components.Features.GoogleButton;

public partial class GoogleButton : ComponentBase, IAsyncDisposable
{
    [Parameter]
    public EventCallback<string> OnLoginSuccess { get; set; }

    [Inject]
    public required IJSRuntime JSRuntime { get; set; }

    [Inject]
    public required IOptions<GoogleOAuthOptions> GoogleOAuthOptions { get; set; }

    private IJSObjectReference? _jsModule;
    private DotNetObjectReference<GoogleButton>? _dotNetRef;
    private bool _isLoading = false;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _dotNetRef ??= DotNetObjectReference.Create(this);
            _jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/Features/GoogleButton/GoogleButton.razor.js");

            var options = GoogleOAuthOptions.Value;

            await _jsModule.InvokeVoidAsync(
                "initialize",
                options.ClientId,
                _dotNetRef);
        }
    }

    private async Task HandleGoogleLogin()
    {
        try
        {
            _isLoading = true;
            StateHasChanged();
            await _jsModule!.InvokeVoidAsync("signIn", _dotNetRef);
        }
        catch
        {
            _isLoading = false;
            StateHasChanged();
        }
    }

    [JSInvokable]
    public async Task OnGoogleCredentialReceived(string idToken)
    {
        if (OnLoginSuccess.HasDelegate)
        {
            await OnLoginSuccess.InvokeAsync(idToken);
        }
    }

    [JSInvokable]
    public void OnGoogleLoginDismissed()
    {
        _isLoading = false;
        StateHasChanged();
    }

    public async ValueTask DisposeAsync()
    {
        _dotNetRef?.Dispose();

        if (_jsModule is not null)
        {
            await _jsModule.DisposeAsync();
        }
    }
}