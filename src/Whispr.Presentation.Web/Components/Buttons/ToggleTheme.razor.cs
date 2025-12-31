using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Whispr.Presentation.Web.Core.Enums;
using Whispr.Presentation.Web.Core.Extensions;
using Whispr.Presentation.Web.Core.Handlers;
using Whispr.Presentation.Web.Core.Managers;
using Whispr.Presentation.Web.Core.Models;

namespace Whispr.Presentation.Web.Components.Buttons;

public partial class ToggleTheme : ComponentBase, IAsyncDisposable
{
    [Inject] 
    public required StateHandler<UserPreferencesModel> UserPreferences { get; set; }
    
    [Inject]
    public required IJSRuntime JSRuntime { get; set; }

    private IJSObjectReference _module = default!;

    protected override async Task OnInitializedAsync()
    {
        await UserPreferences.LoadModelAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>(
                "import",
                "./Components/Buttons/ToggleTheme.razor.js");
            await _module.InvokeVoidAsync("setTheme", UserPreferences.Model.Theme.ToDataBsTheme());
        }
    }

    private string GetIcon()
    {
        return UserPreferences.Model.Theme switch
        {
            Theme.Light => "bi bi-sun-fill",
            Theme.Dark => "bi bi-moon-fill",
            _ => "bi bi-sun-fill"
        };
    }

    private string GetCss()
    {
        return UserPreferences.Model.Theme switch
        {
            Theme.Light => "btn btn-outline-primary",
            Theme.Dark => "btn btn-primary",
            _ => "btn btn-outline-primary"
        };
    }

    private async Task HandleOnClick()
    {
        if (UserPreferences.Model.Theme == Theme.Dark)
        {
            UserPreferences.Model.Theme = Theme.Light;
        }
        else
        {
            UserPreferences.Model.Theme = Theme.Dark;
        }
        await UserPreferences.SaveModelAsync();
        await _module.InvokeVoidAsync("setTheme", UserPreferences.Model.Theme.ToDataBsTheme());
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
        {
            await _module.DisposeAsync();
        }
    }
}