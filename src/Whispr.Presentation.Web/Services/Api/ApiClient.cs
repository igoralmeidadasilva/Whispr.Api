using System.Net.Http.Json;
using System.Text.Json;
using Whispr.Presentation.Web.Core.Http;

namespace Whispr.Presentation.Web.Services.Api;

public sealed class ApiClient : IApiClient
{
    private readonly HttpClient _http;
    private readonly ILogger<ApiClient> _logger;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ApiClient(IHttpClientFactory httpClientFactory, ILogger<ApiClient> logger)
    {
        _http = httpClientFactory.CreateClient(Constants.HttpClients.WhisprApi);
        _logger = logger;
    }

    public Task<ApiResponse<T>> GetAsync<T>(string url, CancellationToken cancellationToken = default)
        => SendCoreAsync<T>(HttpMethod.Get, url, body: null, cancellationToken);

    public Task<ApiResponse<T>> PostAsync<T>(string url, object? body, CancellationToken cancellationToken = default)
        => SendCoreAsync<T>(HttpMethod.Post, url, body, cancellationToken);

    public Task<ApiResponse<NoContent>> PostAsync(string url, object? body, CancellationToken cancellationToken = default)
        => SendCoreAsync(HttpMethod.Post, url, body, cancellationToken);

    public Task<ApiResponse<T>> PutAsync<T>(string url, object? body, CancellationToken cancellationToken = default)
        => SendCoreAsync<T>(HttpMethod.Put, url, body, cancellationToken);

    public Task<ApiResponse<NoContent>> PutAsync(string url, object? body, CancellationToken cancellationToken = default)
        => SendCoreAsync(HttpMethod.Put, url, body, cancellationToken);

    public Task<ApiResponse<T>> PatchAsync<T>(string url, object? body, CancellationToken cancellationToken = default)
        => SendCoreAsync<T>(HttpMethod.Patch, url, body, cancellationToken);

    public Task<ApiResponse<NoContent>> PatchAsync(string url, object? body, CancellationToken cancellationToken = default)
        => SendCoreAsync(HttpMethod.Patch, url, body, cancellationToken);

    public Task<ApiResponse<NoContent>> DeleteAsync(string url, CancellationToken cancellationToken = default)
        => SendCoreAsync(HttpMethod.Delete, url, body: null, cancellationToken);

    private async Task<ApiResponse<T>> SendCoreAsync<T>(
        HttpMethod method,
        string url,
        object? body,
        CancellationToken cancellationToken)
    {
        var request = BuildRequest(method, url, body);
        var response = await _http.SendAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<T>(_jsonOptions, cancellationToken);
            return ApiResponse<T>.Success(data!, response.StatusCode);
        }

        return ApiResponse<T>.Failure(await ReadProblemAsync(response, cancellationToken), response.StatusCode);
    }

    private async Task<ApiResponse<NoContent>> SendCoreAsync(
        HttpMethod method,
        string url,
        object? body,
        CancellationToken cancellationToken)
    {
        var request = BuildRequest(method, url, body);
        var response = await _http.SendAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return ApiResponse<NoContent>.Success(NoContent.Value, response.StatusCode);
        }

        return ApiResponse<NoContent>.Failure(await ReadProblemAsync(response, cancellationToken), response.StatusCode);
    }

    private static HttpRequestMessage BuildRequest(HttpMethod method, string url, object? body)
    {
        var request = new HttpRequestMessage(method, url);
        if (body is not null)
        {
            request.Content = JsonContent.Create(body);
        }

        return request;
    }

    private async Task<ProblemDetails> ReadProblemAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        try
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(_jsonOptions, cancellationToken);
            if (problem is not null)
            {
                return problem;
            }
        }
        catch (JsonException ex)
        {
            _logger.LogWarning(ex, "Error response is not a valid ProblemDetails. Status: {Status}", response.StatusCode);
        }

        return CreateProblem((int)response.StatusCode, "Unexpected error", $"The server returned {(int)response.StatusCode}.");
    }

    private static ProblemDetails CreateProblem(int status, string title, string detail)
    {
        return new()
        {
            Status = status,
            Title = title,
            Detail = detail
        };
    }
}