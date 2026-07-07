using Whispr.Application.Core.Dtos.V1;
using Whispr.Domain.Features.Entities.Users;

namespace Whispr.Application.Core.Mappings;

public static class UserMappings
{
    public static UserDto ToUserDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }
}