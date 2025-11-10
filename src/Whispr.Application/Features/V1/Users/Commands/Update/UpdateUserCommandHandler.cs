using MediatR;
using Microsoft.AspNetCore.Identity;
using Whispr.Application.Core.Helpers;
using Whispr.Application.Core.Interfaces;
using Whispr.Domain.Entities;
using Whispr.SharedKernel.Results;

namespace Whispr.Application.Features.V1.Users.Commands.Update;

internal sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, Result<Unit>>
{
    private readonly UserManager<User> _userManager;

    public UpdateUserCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<Unit>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            return Result<Unit>.Failure(UpdateUserCommandErrors.UserIdNotFound);
        }

        User? findByEmail = await _userManager.FindByEmailAsync(request.Email);
        if (findByEmail != null)
        {
            return Result<Unit>.Failure(UpdateUserCommandErrors.EmailAlreadyExists);
        }

        User? findByName = await _userManager.FindByNameAsync(request.UserName);
        if (findByName != null)
        {
            return Result<Unit>.Failure(UpdateUserCommandErrors.UserNameAlreadyExists);
        }

        IdentityResult setEmailResult = await _userManager.SetEmailAsync(user, request.Email);
        if (!setEmailResult.Succeeded)
        {
            return Result<Unit>.Failure(UpdateUserCommandErrors.SetEmailFailure(IdentityHelper.ToErrorMessage(setEmailResult.Errors)));
        }

        IdentityResult setUserNameResult = await _userManager.SetUserNameAsync(user, request.UserName);
        if (!setUserNameResult.Succeeded)
        {
            return Result<Unit>.Failure(UpdateUserCommandErrors.SetUserNameFailure(IdentityHelper.ToErrorMessage(setUserNameResult.Errors)));
        }

        IdentityResult result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return Result<Unit>.Failure(UpdateUserCommandErrors.IdentityFailure(IdentityHelper.ToErrorMessage(result.Errors)));
        }

        return Result<Unit>.Success();
    }
}