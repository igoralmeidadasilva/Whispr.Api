using Whispr.Domain.Features.Entities.User;
using Whispr.Domain.Features.Models;

namespace Whispr.Domain.Core.Services;

public interface IAuthTokenService
{
    TokenModel GenerateToken(User user);
}