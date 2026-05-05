using Microsoft.Extensions.Options;
using Whispr.Application.Core.Interfaces;
using Whispr.Application.Core.Options;
using Whispr.Domain.Core.Services;
using Whispr.Domain.Features.Entities.User;
using Whispr.Domain.Features.Models;
using Whispr.SharedKernel.Results;

namespace Whispr.Application.Features.V1.Auth.Commands.Login;

internal sealed class LoginCommandHandler : ICommandHandler<LoginCommand, TokenModel>
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IPasswordHasherService _passwordHasherService;
    private readonly IAuthTokenService _authTokenService;

    public LoginCommandHandler(
        IUserReadOnlyRepository userReadOnlyRepository,
        IPasswordHasherService passwordHasherService,
        IAuthTokenService authTokenService)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
        _passwordHasherService = passwordHasherService;
        _authTokenService = authTokenService;
    }

    public async Task<Result<TokenModel>> Handle(LoginCommand request, CancellationToken cancellationToken = default)
    {
        User? user = await _userReadOnlyRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null)
        {
            return Result<TokenModel>.Failure(LoginCommandErrors.EmailNotFound);
        }

        bool isPasswordValid = _passwordHasherService.Verify(request.Password, user.PasswordHash.Hash);

        if (!isPasswordValid)
        {
            return Result<TokenModel>.Failure(LoginCommandErrors.InvalidPassword);
        }

        TokenModel token = _authTokenService.GenerateToken(user);

        return Result<TokenModel>.Success(token);
    }
}