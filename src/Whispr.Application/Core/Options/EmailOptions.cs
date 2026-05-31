namespace Whispr.Application.Core.Options;

public sealed record EmailOptions
{
    public required string SmtpServer { get; init; }
    public required int Port { get; init; }
    public required string EmailAddress { get; init; }
    public required string EmailPassword { get; init; }
}