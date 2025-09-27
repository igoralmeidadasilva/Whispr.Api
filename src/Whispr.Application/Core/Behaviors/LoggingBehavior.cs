using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Whispr.Application.Core.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;
        DateTime timestampStart = DateTime.UtcNow;
        Stopwatch stopwatch = Stopwatch.StartNew();

        try
        {
            _logger.LogInformation(
                "Starting request: {RequestName} at {Timestamp} with payload: {@Request}",
                requestName,
                timestampStart,
                request);

            TResponse response = await next();

            _logger.LogInformation(
                "Request {RequestName} completed successfully with response: {@Response}.",
                requestName,
                response);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Request {RequestName} failed after {ElapsedMilliseconds} ms. Error: {Message}",
                requestName,
                stopwatch.ElapsedMilliseconds,
                ex.Message);
            throw;
        }
        finally
        {
            stopwatch.Stop();
            _logger.LogInformation("Ending request: {RequestName} at {Timestamp}.",
                requestName,
                stopwatch.ElapsedMilliseconds);
        }
    }

}