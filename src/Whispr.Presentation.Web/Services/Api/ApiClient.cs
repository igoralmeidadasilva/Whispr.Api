using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Whispr.Presentation.Web.Components.Features.ProblemModals;
using Whispr.Presentation.Web.Core.Http;
using Whispr.Presentation.Web.Services.Ui.Modal;

namespace Whispr.Presentation.Web.Services.Api;

public sealed class ApiClient : IApiClient
{
    private readonly HttpClient _http;
    private readonly IModalService _modalService;
    private readonly ILogger<ApiClient> _logger;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ApiClient(IHttpClientFactory httpClientFactory, IModalService modalService, ILogger<ApiClient> logger)
    {
        _http = httpClientFactory.CreateClient(Constants.HttpClients.WhisprApi);
        _modalService = modalService;
        _logger = logger;
    }

    public Task<ApiResponse<T>> GetAsync<T>(string url, CancellationToken ct = default)
        => SendCoreAsync<T>(HttpMethod.Get, url, body: null, ct);

    public Task<ApiResponse<T>> PostAsync<T>(string url, object body, CancellationToken ct = default)
        => SendCoreAsync<T>(HttpMethod.Post, url, body, ct);

    public Task<ApiResponse<T>> PutAsync<T>(string url, object body, CancellationToken ct = default)
        => SendCoreAsync<T>(HttpMethod.Put, url, body, ct);

    public Task<ApiResponse<NoContent>> DeleteAsync(string url, CancellationToken ct = default)
        => SendCoreAsync(HttpMethod.Delete, url, body: null, ct);

    private async Task<ApiResponse<T>> SendCoreAsync<T>(
        HttpMethod method,
        string url,
        object? body,
        CancellationToken ct)
    {
        try
        {
            var request = BuildRequest(method, url, body);
            var response = await _http.SendAsync(request, ct);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<T>(_jsonOptions, ct);
                return ApiResponse<T>.Success(data!, response.StatusCode);
            }

            return ApiResponse<T>.Failure(await ReadProblemAsync(response, ct), response.StatusCode);
        }
        catch (TaskCanceledException) when (!ct.IsCancellationRequested)
        {
            var timeout = CreateProblem(504, "Timeout", "O servidor não respondeu a tempo.");
            await _modalService.ShowAsync(CreateModalParameter(timeout));
            return ApiResponse<T>.Failure(timeout, HttpStatusCode.RequestTimeout);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Erro de rede em {Method} {Url}", method, url);
            var network = CreateProblem(500, "Erro de rede", "Verifique sua conexão e tente novamente.");
            await _modalService.ShowAsync(CreateModalParameter(network));
            return ApiResponse<T>.Failure(network, HttpStatusCode.ServiceUnavailable);
        }
    }

    private async Task<ApiResponse<NoContent>> SendCoreAsync(
        HttpMethod method,
        string url,
        object? body,
        CancellationToken ct)
    {
        try
        {
            var request = BuildRequest(method, url, body);
            var response = await _http.SendAsync(request, ct);

            if (response.IsSuccessStatusCode)
            {
                return ApiResponse<NoContent>.Success(NoContent.Value, response.StatusCode);
            }

            return ApiResponse<NoContent>.Failure(await ReadProblemAsync(response, ct), response.StatusCode);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Erro de rede em {Method} {Url}", method, url);
            var network = CreateProblem(500, "Erro de rede", "Verifique sua conexão e tente novamente.");
            await _modalService.ShowAsync(CreateModalParameter(network));
            return ApiResponse<NoContent>.Failure(network, HttpStatusCode.ServiceUnavailable);
        }
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

    private async Task<ProblemDetails> ReadProblemAsync(HttpResponseMessage response, CancellationToken ct)
    {
        try
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(_jsonOptions, ct);
            if (problem is not null)
            {
                return problem;
            }
        }
        catch (JsonException ex)
        {
            _logger.LogWarning(ex, "Resposta de erro não é um ProblemDetails válido. Status: {Status}", response.StatusCode);
        }

        return CreateProblem((int)response.StatusCode, "Erro inesperado", $"O servidor retornou {(int)response.StatusCode}.");
    }

    private static ProblemDetails CreateProblem(int status, string title, string detail)
    {
        return new ProblemDetails
        { 
            Status = status,
            Title = title,
            Detail = detail
        };
    }

    private static ProblemModalParameters CreateModalParameter(ProblemDetails problemDetails)
    {
        return new ProblemModalParameters
        {
            Title = problemDetails.Title,
            Problem = problemDetails.Detail
        };
    }
}