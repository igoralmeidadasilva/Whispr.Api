using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Whispr.Presentation.Web.Components.Ui.Inputs;

public partial class DigitInput : InputBase<string>, IAsyncDisposable
{
    [Parameter]
    public string? Id { get; set; } = Guid.NewGuid().ToString();

    [Parameter]
    public string? HelpText { get; set; }

    [Parameter]
    public new string? CssClass { get; set; }

    [Parameter]
    public bool IsDisabled { get; set; }

    [Parameter]
    public bool IsRequired { get; set; }

    [Parameter]
    public bool ShowValidationMessage { get; set; } = true;

    [Inject]
    public required IJSRuntime JSRuntime { get; set; }

    private ElementReference? _element;
    private IJSObjectReference? _module;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/Ui/Inputs/DigitInput.razor.js");

            await _module.InvokeVoidAsync("initialize", _element);
        }
    }

    protected string BuildCssClass()
    {
        StringBuilder css = new ("form-control text-center");
        if (!string.IsNullOrWhiteSpace(CssClass))
        {
            css.Append($" {CssClass}");
        }
        return css.ToString();
    }

    protected override bool TryParseValueFromString(
        string? value,
        [MaybeNullWhen(false)] out string result,
        [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (string.IsNullOrEmpty(value))
        {
            result = string.Empty;
            validationErrorMessage = null;
            return true;
        }

        if (value.Length == 1 && char.IsDigit(value[0]))
        {
            result = value;
            validationErrorMessage = null;
            return true;
        }

        result = default!;
        validationErrorMessage = "O campo deve conter apenas um dígito (0–9).";
        return false;
    }

    private void HandleChange(ChangeEventArgs e)
    {
        CurrentValueAsString = e.Value?.ToString();
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("dispose", _element);
            await _module.DisposeAsync();
        }
    }
}