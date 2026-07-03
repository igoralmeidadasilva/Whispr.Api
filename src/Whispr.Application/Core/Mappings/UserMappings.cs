using Whispr.Application.Core.Models.V1;
using Whispr.Domain.Features.Entities.Users;

namespace Whispr.Application.Core.Mappings;

public static class UserMappings
{
    public static UserDto ToUserDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Name,
            Email = user.Email
        };
    }
}