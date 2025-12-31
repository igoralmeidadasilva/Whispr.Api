namespace Whispr.Presentation.Web.Core.Managers;

public class ExpirationModel<T>
{
    public T? Value { get; set; }
    public DateTime ExpirationAtUtc { get; set; }
    public bool IsValid => Value is not null && DateTime.UtcNow < ExpirationAtUtc;
    public bool IsInvalid() => !IsValid;
}