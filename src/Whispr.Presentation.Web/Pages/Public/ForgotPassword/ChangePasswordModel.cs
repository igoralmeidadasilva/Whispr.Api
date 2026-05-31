namespace Whispr.Presentation.Web.Pages.Public.ForgotPassword;

public sealed class ChangePasswordModel
{
    public string? Email { get; set; }
    public string? DigitOne { get; set; }
    public string? DigitTwo { get; set; }
    public string? DigitThree { get; set; }
    public string? DigitFour { get; set; }
    public string? RecoveryCode => $"{DigitOne}{DigitTwo}{DigitThree}{DigitFour}";
    public string? NewPassword { get; set; }
    public string? ConfirmNewPassword { get; set; }
}