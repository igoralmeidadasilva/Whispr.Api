using Whispr.Application.Core.Models.V1;
using Whispr.SharedKernel.Results.Models;

namespace Whispr.Application.Core.Services;

public interface IStorageService
{
    Task<Result<MediaFileDto>> DownloadAsync(string containerName, string fileName, CancellationToken cancellationToken = default);
    Task<Result<NoValue>> UploadAsync(string containerName, string fileName, Stream fileStream, CancellationToken cancellationToken = default);
    Task<Result<NoValue>> DeleteAsync(string containerName, string fileName, CancellationToken cancellationToken = default);
}