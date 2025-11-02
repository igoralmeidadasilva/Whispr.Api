using Microsoft.AspNetCore.Identity;
using Whispr.Application.Core.Interfaces;
using Whispr.Domain.Entities;
using Whispr.SharedKernel.Results;

namespace Whispr.Application.Commands.Users;

internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Result<Guid>>
{
    public UserManager<User> _userManager;

    public CreateUserCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        User newUser = new()
        {
            UserName = request.Username,
            Email = request.Email,
            PasswordHash = request.Password
        };

        IdentityResult result = await _userManager.CreateAsync(newUser);

        if (!result.Succeeded)
        {
            var error = string.Join(" | ", result.Errors.Select(x => $"{x.Code}: {x.Description}"));
            return Result<Guid>.Failure(Error.Create("User.CreateFailure", error, ErrorType.Failure));
        }
        return Result<Guid>.Success(Guid.Parse(newUser.Id));
    }
}