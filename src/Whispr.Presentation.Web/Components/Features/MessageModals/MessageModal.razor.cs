using Microsoft.AspNetCore.Components;
using System.Drawing;
using Whispr.Presentation.Web.Components.Features.MessageAlerts;
using Whispr.Presentation.Web.Components.Ui.Alerts;
using Whispr.Presentation.Web.Components.Ui.Modals;
using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Components.Features.MessageModals;

public partial class MessageModal : ComponentBase
{
    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public Colors HeaderColor { get; set; } = Colors.None;

    private Modal? _modal;
    private string? _message;
    private string? _correlationId;
    private IEnumerable<string> _errors = [];

    private string ExtractCorrelationId(Dictionary<string, object>? extensions)
    {
        return Guid.NewGuid().ToString();
    }
    private IEnumerable<string> ExtractErrorList(Dictionary<string, string[]>? errors)
    {
        return ["error-1", "error-2", "error-3"];
    }

    public async Task ShowAsync(MessageModalParameters parameters)
    {
        if (_modal is not null)
        {
            await InvokeAsync(() =>
            {
                HeaderColor = parameters.HeaderColor;
                Title = parameters.ProblemDetails.Title;
                _message = parameters.ProblemDetails.Detail;
                _correlationId = ExtractCorrelationId(parameters.ProblemDetails.Extensions);
                _errors = ExtractErrorList(parameters.ProblemDetails.Errors);
                StateHasChanged();
            });

            await _modal.ShowAsync();
        }
    }

    public async Task HideAsync()
    {
        if (_modal is not null)
        {
            await InvokeAsync(() =>
            {
                HeaderColor = Colors.None;
                Title = string.Empty;
                _message = string.Empty;
                _correlationId = string.Empty;
                _errors = [];
                StateHasChanged();
            });

            await _modal.HideAsync();
        }
    }
}