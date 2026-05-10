using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Whispr.Presentation.Web.Components.Ui.Inputs;

public partial class EmailInput : InputBase<string>
{
    [Parameter]
    [EditorRequired]
    public string Label { get; set; } = string.Empty;

    [Parameter]
    public string? Id { get; set; } = Guid.NewGuid().ToString();

    [Parameter]
    public string? HelpText { get; set; }

    [Parameter]
    public string WrapperCssClass { get; set; } = string.Empty;

    [Parameter]
    public bool IsDisabled { get; set; }

    [Parameter]
    public bool IsRequired { get; set; }

    private string[] _emailSuggestions = ["gmail.com", "hotmail.com", "outlook.com", "yahoo.com", "icloud.com"];

    protected string? BuildCssClass()
    {
        var css = new StringBuilder("form-control");
        if (!string.IsNullOrWhiteSpace(CssClass))
        {
            css.Append($" {CssClass}");
        }
        return css.ToString();
    }

    protected string? BuildWrapperCssClass()
    {
        var css = new StringBuilder("form-floating");
        if (!string.IsNullOrWhiteSpace(WrapperCssClass))
        {
            css.Append($" {WrapperCssClass}");
        }
        return css.ToString();
    }

    protected override bool TryParseValueFromString(
        string? value,
        [MaybeNullWhen(false)] out string result,
        [NotNullWhen(false)] out string? validationErrorMessage)
    {
        return BindConverter.TryConvertTo(value, System.Globalization.CultureInfo.CurrentCulture, out result)
            ? Success(out validationErrorMessage)
            : Failure($"Invalid value for '{Label}'.", out result, out validationErrorMessage);
    }

    private void HandleInput(ChangeEventArgs e)
    {
        CurrentValueAsString = e.Value?.ToString();
    }

    private void HandleChange(ChangeEventArgs e)
    {
        CurrentValueAsString = e.Value?.ToString();
    }

    private static bool Success(out string? errorMessage)
    {
        errorMessage = null;
        return true;
    }

    private static bool Failure(
        string message,
        [MaybeNullWhen(false)] out string result,
        out string? errorMessage)
    {
        result = default;
        errorMessage = message;
        return false;
    }
}