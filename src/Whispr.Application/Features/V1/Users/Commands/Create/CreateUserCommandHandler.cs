using MediatR;
using Microsoft.AspNetCore.Identity;
using Whispr.Application.Core.Interfaces;
using Whispr.Domain.Entities;
using Whispr.SharedKernel.Results;

namespace Whispr.Application.Features.V1.Users.Commands.Create;

internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Result<Unit>>
{
    private readonly UserManager<User> _userManager;

    public CreateUserCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<Unit>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        User? findByEmail = await _userManager.FindByEmailAsync(request.Email);
        if (findByEmail != null)
        {
            return Result<Unit>.Failure(Error.Create("CreateUserCommand.Email.AlreadyExists", CreateUserCommandValidationErrors.EmailAlreadyExists, ErrorType.Conflict));
        }
        
        User? findByName = await _userManager.FindByNameAsync(request.Username);
        if (findByEmail != null)
        {
            return Result<Unit>.Failure(Error.Create("CreateUserCommand.UserName.AlreadyExists", CreateUserCommandValidationErrors.UserNameAlreadyExists, ErrorType.Conflict));
        }
        
        User newUser = new()
        {
            UserName = request.Username,
            Email = request.Email,
            PasswordHash = request.Password
        };

        IdentityResult result = await _userManager.CreateAsync(newUser);

        if (!result.Succeeded)
        {
            var error = string.Join("\n", result.Errors.Select(x => $"{x.Code}: {x.Description}"));
            return Result<Unit>.Failure(Error.Create("CreateUserCommand.Failure", error));
        }
        return Result<Unit>.Success();
    }
}