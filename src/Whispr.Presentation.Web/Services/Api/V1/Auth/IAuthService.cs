using Whispr.Presentation.Web.Core.Dtos;
using Whispr.Presentation.Web.Services.Api.V1.Auth.Requests;

namespace Whispr.Presentation.Web.Services.Api.V1.Auth;

public interface IAuthService
{
    Task<AuthTokenDto?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<AuthTokenDto?> RefreshAsync(RefreshRequest request, CancellationToken cancellationToken = default);
}