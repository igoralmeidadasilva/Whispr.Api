using Microsoft.AspNetCore.Identity;
using Whispr.Application.Core.Interfaces;
using Whispr.Application.Core.Models.V1;
using Whispr.Domain.Entities;
using Whispr.SharedKernel.Results;

namespace Whispr.Application.Features.V1.Users.Queries.GetById;

internal sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, Result<UserDto>>
{
    private readonly UserManager<User> _userManager;

    public GetUserByIdQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        User? user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            return Result<UserDto>.Failure(GetUserByIdQueryErrors.UserNotFound);
        }
        var userDto = new UserDto
        {
            Id = Guid.Parse(user.Id),
            Username = user.UserName!,
            Email = user.Email!
        };
        return Result<UserDto>.Success(userDto);
    }
}