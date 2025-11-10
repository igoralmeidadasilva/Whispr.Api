using Microsoft.AspNetCore.Identity;
using Whispr.Application.Core.Interfaces;
using Whispr.Application.Core.Models.V1;
using Whispr.Domain.Entities;
using Whispr.SharedKernel.Results;

namespace Whispr.Application.Features.V1.Users.Queries.GetUsers;

internal sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, Result<IEnumerable<UserDto>>>
{
    private readonly UserManager<User> _userManager;

    public GetUsersQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public Task<Result<IEnumerable<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        List<User> users = _userManager.Users.ToList();

        if (users.Count == 0)
        {
            var errorResponse = Result<IEnumerable<UserDto>>.Failure(GetUsersQueryErrors.NoUsersFound);
            return Task.FromResult(errorResponse);
        }
        var usersDto = users.Select(user => new UserDto
        {
            Id = Guid.Parse(user.Id),
            Username = user.UserName!,
            Email = user.Email!
        });
        var response = Result<IEnumerable<UserDto>>.Success(usersDto);
        return Task.FromResult(response);
    }
}