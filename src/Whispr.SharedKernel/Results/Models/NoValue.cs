namespace Whispr.SharedKernel.Results.Models;

public record NoValue
{
    public static readonly NoValue Value = new();
    private NoValue() { }
}