using Whispr.Application.Core.Dtos.V1;

namespace Whispr.Application.Core.Providers;

public interface ICurrentUserProvider
{
    public CurrentUserDto? GetCurrentUser();
}