using Microsoft.AspNetCore.Components;
using Whispr.Presentation.Web.Components.Ui.Alerts;
using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Components.Features.MessageAlerts;

public partial class MessageAlert : ComponentBase
{
    [Parameter]
    public required string Id { get; set; } = Guid.NewGuid().ToString();

    [Parameter]
    public Colors Color { get; set; } = Colors.None;

    [Parameter]
    public IconClasses IconClass { get; set; } = IconClasses.None;

    [Parameter]
    public string? CssClass { get; set; }

    [Inject]
    public required ILogger<MessageAlert> Logger { get; set; }

    private Alert _alert = default!;
    private string? _text;

    public async ValueTask ShowAsync(MessageAlertParameters alertParameters)
    {
        if (_alert is null)
        {
            Logger.LogDebug("Alert component reference is null. Cannot show message alert.");
            return;
        }

        await InvokeAsync(() =>
        {
            Id = alertParameters.Id;
            Color = alertParameters.Color;
            IconClass = alertParameters.IconClass;
            _text = alertParameters.Message;
            StateHasChanged();
        });

        _alert.Show();
    }

    public async ValueTask HideAsync()
    {
        if (_alert is null)
        {
            Logger.LogDebug("Alert component reference is null. Cannot hide message alert.");
            return;
        }

        await InvokeAsync(() =>
        {
            Color = Colors.None;
            IconClass = IconClasses.None;
            _text = string.Empty;
            StateHasChanged();
        });

        _alert.Hide();
    }
}