using System.Security.Claims;
using Whispr.Domain.Features.Entities.User;
using Whispr.Domain.Features.Models;

namespace Whispr.Domain.Core.Services;

public interface IAuthTokenService
{
    TokenModel GenerateAccessToken(User user);
    TokenModel GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromAccessToken(string accessToken);
}