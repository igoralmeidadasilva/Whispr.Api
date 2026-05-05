namespace Whispr.SharedKernel.Results;

public sealed record ValidationError : Error
{
    public Dictionary<string, string[]> Failures { get; init; }

    private ValidationError(Dictionary<string, string[]> failures, string code, string message, ErrorType errorType)
        : base(code, message, errorType)
    {
        Failures = failures;
    }

    public static ValidationError Create(Dictionary<string, string[]> failures, string code, string message)
    {
        return new (failures, code, message, ErrorType.Validation);
    }
}