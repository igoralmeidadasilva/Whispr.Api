namespace Whispr.SharedKernel.Results;

public record NoValue
{
    public static readonly NoValue Instance = new();
    private NoValue() { }
}