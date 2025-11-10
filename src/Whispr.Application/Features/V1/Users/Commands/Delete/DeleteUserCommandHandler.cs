using MediatR;
using Microsoft.AspNetCore.Identity;
using Whispr.Application.Core.Helpers;
using Whispr.Application.Core.Interfaces;
using Whispr.Domain.Entities;
using Whispr.SharedKernel.Results;

namespace Whispr.Application.Features.V1.Users.Commands.Delete;

internal sealed class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, Result<Unit>>
{
    private readonly UserManager<User> _userManager;

    public DeleteUserCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    public async Task<Result<Unit>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            return Result<Unit>.Failure(DeleteUserCommandErrors.UserIdNotFound);
        }

        IdentityResult result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            return Result<Unit>.Failure(DeleteUserCommandErrors.IdentityFailure(IdentityHelper.ToErrorMessage(result.Errors)));
        }

        return Result<Unit>.Success();
    }
}