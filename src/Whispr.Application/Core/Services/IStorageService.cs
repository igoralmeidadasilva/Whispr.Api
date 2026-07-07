namespace Whispr.Application.Core.Services;

public interface IStorageService
{
    Result<string> GetSasUri(string containerName, string fileName, TimeSpan expiresIn);
    Task<Result<NoValue>> UploadAsync(string containerName, string fileName, Stream fileStream, string contentType, CancellationToken cancellationToken = default);
    Task<Result<NoValue>> DeleteAsync(string containerName, string fileName, CancellationToken cancellationToken = default);
}