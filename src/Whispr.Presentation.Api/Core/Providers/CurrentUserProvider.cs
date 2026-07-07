using System.Security.Claims;
using Whispr.Application.Core.Interfaces;
using Whispr.Application.Core.Models.V1;

namespace Whispr.Presentation.Api.Core.Providers;

public sealed class CurrentUserProvider : ICurrentUserProvider
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