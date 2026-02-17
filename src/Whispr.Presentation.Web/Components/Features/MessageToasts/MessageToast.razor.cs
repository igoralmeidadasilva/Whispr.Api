using Microsoft.AspNetCore.Components;
using Whispr.Presentation.Web.Core.Enums;

namespace Whispr.Presentation.Web.Components.Features.MessageToasts;

public partial class MessageToast : ComponentBase
{
    [Parameter]
    public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.End;

    [Parameter]
    public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Bottom;

    [Parameter]
    public List<MessageToastParameters> Toasts { get; set; } = [];

    public async Task CreateAsync(MessageToastParameters toastParameter)
    {
        await InvokeAsync(() =>
        {
            Toasts.Add(toastParameter);
            StateHasChanged();
        });
    }

    private async Task HandleHidden(string id)
    {
        if (Toasts.Count == 0)
        {
            return;
        }

        var toast = Toasts.FirstOrDefault(x => string.Equals(x.Id, id));

        if (toast is not null)
        {
            Toasts.Remove(toast);
        }
    }
}