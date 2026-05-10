namespace Whispr.Presentation.Web.Core.Handlers.HttpClient;

public sealed class LoggingDelegatingHandler : DelegatingHandler
{
    private readonly ILogger<LoggingDelegatingHandler> _logger;

    public LoggingDelegatingHandler(ILogger<LoggingDelegatingHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Before HTTP request");

            var result = await base.SendAsync(request, cancellationToken);

            _logger.LogInformation("After HTTP request");

            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "HTTP request failed");

            throw;
        }
    }
}