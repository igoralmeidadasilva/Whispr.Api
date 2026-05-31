namespace Whispr.SharedKernel.Results.Errors;

public record Error
{
    public string Code { get; init; }
    public string Message { get; init; }
    public ErrorType Type { get; init; }

    protected Error(string code, string message, ErrorType type)
    {
        Code = code;
        Message = message;
        Type = type;
    }

    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);
    public static Error Create(string code, string message, ErrorType type = ErrorType.Failure) => new(code, message, type);
}