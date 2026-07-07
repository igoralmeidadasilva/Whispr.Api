using Whispr.Application.Core.Models.V1;

namespace Whispr.Application.Core.Interfaces;

public interface ICurrentUserProvider
{
    public CurrentUserDto? GetCurrentUser();
}