using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Whispr.Presentation.Web.Core.Enums;
using Whispr.Presentation.Web.Core.Extensions;
using Whispr.Presentation.Web.Core.Handlers;
using Whispr.Presentation.Web.Core.Managers;
using Whispr.Presentation.Web.Core.Models;

namespace Whispr.Presentation.Web.Components.Ui.Buttons;

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
            await _module.InvokeVoidAsync("setTheme", UserPreferences.Model.Theme.ToCss());
        }
    }

    private string GetIcon()
    {
        return UserPreferences.Model.Theme switch
        {
            Themes.Light => "bi bi-sun-fill",
            Themes.Dark => "bi bi-moon-fill",
            _ => "bi bi-sun-fill"
        };
    }

    private string GetCss()
    {
        return UserPreferences.Model.Theme switch
        {
            Themes.Light => "btn btn-outline-primary",
            Themes.Dark => "btn btn-primary",
            _ => "btn btn-outline-primary"
        };
    }

    private async Task HandleOnClick()
    {
        if (UserPreferences.Model.Theme == Themes.Dark)
        {
            UserPreferences.Model.Theme = Themes.Light;
        }
        else
        {
            UserPreferences.Model.Theme = Themes.Dark;
        }
        await UserPreferences.SaveModelAsync();
        await _module.InvokeVoidAsync("setTheme", UserPreferences.Model.Theme.ToCss());
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
        {
            await _module.DisposeAsync();
        }
    }
}