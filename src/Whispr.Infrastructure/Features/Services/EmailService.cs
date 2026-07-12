using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Diagnostics;
using Whispr.Application.Core.Options;
using Whispr.Application.Core.Services;
using Whispr.SharedKernel.Results;
using Whispr.SharedKernel.Results.Errors;
using Whispr.SharedKernel.Results.Models;

namespace Whispr.Infrastructure.Features.Services;

internal sealed class EmailService : IEmailService
{
    private readonly EmailOptions _options;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailOptions> options, ILogger<EmailService> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task<Result<NoValue>> SendAsync(
        string toEmail,
        string toName,
        string subject,
        string body,
        CancellationToken cancellationToken = default)
    {
        DateTime timestampStart = DateTime.UtcNow;
        Stopwatch stopwatch = Stopwatch.StartNew();

        try
        {
            _logger.LogInformation(
                "Starting sending email to {ToEmail} at {Timestamp}.",
                toEmail,
                timestampStart);

            MimeMessage email = CreateEmailBody(toEmail, toName, subject, body);

            using SmtpClient client = new();

            await client.ConnectAsync(_options.SmtpServer, _options.Port, false, cancellationToken);
            await client.AuthenticateAsync(_options.EmailAddress, _options.EmailPassword, cancellationToken);

            await client.SendAsync(email, cancellationToken);

            await client.DisconnectAsync(true, cancellationToken);

            _logger.LogInformation(
                "The sending email to {ToEmail} was completed successfully.",
                toEmail);

            return Result<NoValue>.Success(NoValue.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Sending email to {ToEmail} failed after {ElapsedMilliseconds} ms. Error: {Message}",
                toEmail,
                stopwatch.ElapsedMilliseconds,
                ex.Message);

            return Result<NoValue>.Failure(Error.Create("EmailService.SendAsync", "Failed to send email."));
        }
        finally
        {
            stopwatch.Stop();

            _logger.LogInformation("Ending sending email to {ToEmail} at {Timestamp}.",
                toEmail,
                stopwatch.ElapsedMilliseconds);
        }
    }

    private MimeMessage CreateEmailBody(
        string to,
        string toName,
        string subject,
        string body)
    {
        MimeMessage message = new();
        message.From.Add(new MailboxAddress("Whispr team", _options.EmailAddress));
        message.To.Add(new MailboxAddress(toName, to));
        message.Subject = subject;

        message.Body = new TextPart("html")
        {
            Text = body
        };

        return message;
    }
}