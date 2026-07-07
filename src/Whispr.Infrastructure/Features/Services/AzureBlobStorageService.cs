using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Whispr.Application.Core.Models.V1;
using Whispr.Application.Core.Options;
using Whispr.Application.Core.Services;
using Whispr.SharedKernel.Results;
using Whispr.SharedKernel.Results.Errors;
using Whispr.SharedKernel.Results.Models;

namespace Whispr.Infrastructure.Features.Services;

internal sealed class AzureBlobStorageService : IStorageService
{
    private readonly ILogger<AzureBlobStorageService> _logger;
    private readonly StorageOptions _storageOptions;
    private readonly BlobServiceClient _blobServiceClient;

    public AzureBlobStorageService(ILogger<AzureBlobStorageService> logger, IOptions<StorageOptions> storageOptions, BlobServiceClient blobServiceClient)
    {
        _logger = logger;
        _storageOptions = storageOptions.Value;
        _blobServiceClient = blobServiceClient;
    }

    public async Task<Result<MediaFileDto>> DownloadAsync(string containerName, string fileName, CancellationToken cancellationToken = default)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            if (!await blobClient.ExistsAsync())
            {
                throw new FileNotFoundException($"O arquivo {fileName} não foi encontrado no Azure Blob Storage.");
            }

            Azure.Response<BlobDownloadResult> downloadResult = await blobClient.DownloadContentAsync(cancellationToken);
        
            var mediaFileDto = new MediaFileDto
            {
                Value = downloadResult.Value.Content.ToArray(),
                ContentType = downloadResult.Value.Details.ContentType,
                Name = fileName
            };

            return Result<MediaFileDto>.Success(mediaFileDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while uploading the file to Azure Blob Storage.");

            return Result<MediaFileDto>.Failure(Error.Create("StorageError", "An error occurred while uploading the file to Azure Blob Storage."));
        }
    }

    public async Task<Result<NoValue>> DeleteAsync(string containerName, string fileName, CancellationToken cancellationToken = default)
    {
        try
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            Azure.Response response = await blobClient.DeleteAsync(cancellationToken: cancellationToken);
            
            return Result<NoValue>.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while uploading the file to Azure Blob Storage.");

            return Result<NoValue>.Failure(Error.Create("StorageError", "An error occurred while uploading the file to Azure Blob Storage."));
        }
    }

    public async Task<Result<NoValue>> UploadAsync(string containerName, string fileName, Stream fileStream, CancellationToken cancellationToken = default)
    {
        try
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            Azure.Response<BlobContentInfo> response = await blobClient.UploadAsync(fileStream, overwrite: true, cancellationToken: cancellationToken);

            return Result<NoValue>.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while uploading the file to Azure Blob Storage.");

            return Result<NoValue>.Failure(Error.Create("StorageError", "An error occurred while uploading the file to Azure Blob Storage."));
        }
    }
}