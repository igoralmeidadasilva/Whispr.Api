namespace Whispr.SharedKernel.Results.Models;

public record NoValue
{
    public static readonly NoValue Instance = new();
    private NoValue() { }
}