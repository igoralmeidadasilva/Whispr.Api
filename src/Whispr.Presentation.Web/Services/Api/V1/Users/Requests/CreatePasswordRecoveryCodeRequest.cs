namespace Whispr.Presentation.Web.Services.Api.V1.Users.Requests;

public sealed record CreatePasswordRecoveryCodeRequest
{
    public required string Email { get; set; }
}