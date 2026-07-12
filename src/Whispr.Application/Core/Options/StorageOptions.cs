namespace Whispr.Application.Core.Options;

public sealed record StorageOptions
{
    public required string ContainerName { get; init; }
}