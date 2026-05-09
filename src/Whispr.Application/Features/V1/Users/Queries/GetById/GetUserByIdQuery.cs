using Whispr.Application.Core.Models.V1;

namespace Whispr.Application.Features.V1.Users.Queries.GetById;

public sealed record GetUserByIdQuery : IQuery<UserDto>
{
    public Guid UserId { get; init; }
}