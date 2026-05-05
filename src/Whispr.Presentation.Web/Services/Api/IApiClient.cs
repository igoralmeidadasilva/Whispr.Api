using Whispr.Presentation.Web.Core.Http;

namespace Whispr.Presentation.Web.Services.Api;

public interface IApiClient
{
    Task<ApiResponse<T>> GetAsync<T>(string url, CancellationToken cancellationToken = default);
    Task<ApiResponse<T>> PostAsync<T>(string url, object body, CancellationToken cancellationToken = default);
    Task<ApiResponse<T>> PutAsync<T>(string url, object body, CancellationToken cancellationToken = default);
    Task<ApiResponse<NoContent>> DeleteAsync(string url, CancellationToken ccancellationTokent = default);
}