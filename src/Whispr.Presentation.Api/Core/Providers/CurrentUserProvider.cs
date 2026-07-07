using System.Security.Claims;
using Whispr.Application.Core.Dtos.V1;
using Whispr.Application.Core.Providers;

namespace Whispr.Presentation.Api.Core.Providers;

internal sealed class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public CurrentUserDto? GetCurrentUser()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext?.User?.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        string? userId = httpContext.User.FindFirstValue(ClaimTypes.Sid);
        string? userName = httpContext.User.FindFirstValue(ClaimTypes.Name);
        string? userEmail = httpContext.User.FindFirstValue(ClaimTypes.Email);

        return new CurrentUserDto
        {
            Id = Guid.Parse(userId!),
            Name = userName!,
            Email = userEmail!
        };
    }
}