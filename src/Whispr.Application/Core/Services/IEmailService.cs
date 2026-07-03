using Whispr.SharedKernel.Results.Models;

namespace Whispr.Application.Core.Services;

public interface IEmailService
{
    public Task<Result<NoValue>> SendAsync(
        string toEmail,
        string toName,
        string subject,
        string body,
        CancellationToken cancellationToken = default);
}