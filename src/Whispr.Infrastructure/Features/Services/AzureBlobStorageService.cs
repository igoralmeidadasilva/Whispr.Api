using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
    private const string BLOB_RESOURCE_NAME = "b";

    public AzureBlobStorageService(ILogger<AzureBlobStorageService> logger, IOptions<StorageOptions> storageOptions, BlobServiceClient blobServiceClient)
    {
        _logger = logger;
        _storageOptions = storageOptions.Value;
        _blobServiceClient = blobServiceClient;
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
            _logger.LogError(ex, "An error occurred while deleting the file to Azure Blob Storage.");

            return Result<NoValue>.Failure(Error.Create("StorageError", "An error occurred while deleting the file to Azure Blob Storage."));
        }
    }

    public async Task<Result<NoValue>> UploadAsync(
        string containerName,
        string fileName,
        Stream fileStream,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        try
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            BlobUploadOptions blobOptions = new()
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = contentType 
                },
            };
            Azure.Response<BlobContentInfo> response = await blobClient.UploadAsync(
                fileStream,
                blobOptions,
                cancellationToken);

            return Result<NoValue>.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while uploading the file to Azure Blob Storage.");

            return Result<NoValue>.Failure(Error.Create("StorageError", "An error occurred while uploading the file to Azure Blob Storage."));
        }
    }

    public Result<string> GetSasUri(string containerName, string fileName, TimeSpan expiresIn)
    {        
        try
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            BlobSasBuilder sasBuilder = new()
            {
                BlobContainerName = containerName,
                BlobName = fileName,
                Resource = BLOB_RESOURCE_NAME,
                ExpiresOn = DateTimeOffset.UtcNow.Add(expiresIn)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            if (blobClient.CanGenerateSasUri)
            {
                Uri sasUri = blobClient.GenerateSasUri(sasBuilder);
                return Result<string>.Success(sasUri.ToString());
            }

            return Result<string>.Failure(Error.Create("StorageError", "It was not possible to generate the SAS URI with the current credentials."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating SAS token for file {FileName}", fileName);

            return Result<string>.Failure(Error.Create("StorageError", "Error generating secure URL for the file."));
        }
    }
}