using System.Net;
using System.Text;
using System.Text.Json;
using Whispr.Presentation.Web.Core.Http;

namespace Whispr.Presentation.Web.Core.Handlers.HttpClient;

public sealed class GlobalErrorDelegatingHandler : DelegatingHandler
{
    private readonly ILogger<GlobalErrorDelegatingHandler> _logger;

    public GlobalErrorDelegatingHandler(ILogger<GlobalErrorDelegatingHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        try
        {
            return await base.SendAsync(request, cancellationToken);
        }
        catch (TimeoutException ex)
        {
            _logger.LogError(ex,
                "Request timed out while sending {Method} to {Uri}: {Message}",
                request.Method,
                request.RequestUri,
                ex.Message);

            var problem = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.4",
                Title = "Request Timeout",
                Status = 503,
                Detail = "The request timed out. Please try again later.",
                Instance = request.RequestUri?.ToString(),
            };

            var json = JsonSerializer.Serialize(problem);

            return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/problem+json"),
                RequestMessage = request
            };
        }
        catch (TaskCanceledException ex) when (ex.CancellationToken != cancellationToken)
        {
            _logger.LogError(ex,
                "Request timed out (HttpClient timeout) while sending {Method} to {Uri}: {Message}",
                request.Method,
                request.RequestUri,
                ex.Message);

            var problem = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.4",
                Title = "Request Timeout",
                Status = 503,
                Detail = "The request timed out. Please try again later.",
                Instance = request.RequestUri?.ToString(),
            };

            var json = JsonSerializer.Serialize(problem);

            return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/problem+json"),
                RequestMessage = request
            };
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex,
                "HTTP request failed while sending {Method} to {Uri} with status {StatusCode}: {Message}",
                request.Method,
                request.RequestUri,
                ex.StatusCode,
                ex.Message);

            var statusCode = ex.StatusCode is not null
                ? (int)ex.StatusCode
                : 502;

            var problem = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.3",
                Title = "Bad Gateway",
                Status = statusCode,
                Detail = "An error occurred while processing your request. Please try again later.",
                Instance = request.RequestUri?.ToString(),
            };

            var json = JsonSerializer.Serialize(problem);

            return new HttpResponseMessage(ex.StatusCode ?? HttpStatusCode.BadGateway)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/problem+json"),
                RequestMessage = request
            };
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex,
                "Unexpected error while sending {Method} to {Uri}: {Message}",
                request.Method,
                request.RequestUri,
                ex.Message);

            var problem = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Title = "Internal Server Error",
                Status = 500,
                Detail = "An unexpected error occurred. Please try again later.",
                Instance = request.RequestUri?.ToString(),
            };

            var json = JsonSerializer.Serialize(problem);

            return new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/problem+json"),
                RequestMessage = request
            };
        }
    }
}