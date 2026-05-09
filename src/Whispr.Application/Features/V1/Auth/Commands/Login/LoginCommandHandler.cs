using Whispr.Application.Core.Models.V1;
using Whispr.Domain.Core.Interfaces;
using Whispr.Domain.Core.Services;
using Whispr.Domain.Features.Entities.RefreshToken;
using Whispr.Domain.Features.Entities.User;
using Whispr.Domain.Features.Models;

namespace Whispr.Application.Features.V1.Auth.Commands.Login;

internal sealed class LoginCommandHandler : ICommandHandler<LoginCommand, AuthTokenDto>
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IRefreshTokenPersistenceRepository _refreshTokenPersistenceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasherService _passwordHasherService;
    private readonly IAuthTokenService _authTokenService;

    public LoginCommandHandler(
        IUserReadOnlyRepository userReadOnlyRepository,
        IRefreshTokenPersistenceRepository refreshTokenPersistenceRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasherService passwordHasherService,
        IAuthTokenService authTokenService)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
        _refreshTokenPersistenceRepository = refreshTokenPersistenceRepository;
        _unitOfWork = unitOfWork;
        _passwordHasherService = passwordHasherService;
        _authTokenService = authTokenService;
    }

    public async Task<Result<AuthTokenDto>> Handle(LoginCommand request, CancellationToken cancellationToken = default)
    {
        User? user = await _userReadOnlyRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null)
        {
            return Result<AuthTokenDto>.Failure(LoginCommandErrors.EmailNotFound);
        }

        bool isPasswordValid = _passwordHasherService.Verify(request.Password, user.PasswordHash.Hash);

        if (!isPasswordValid)
        {
            return Result<AuthTokenDto>.Failure(LoginCommandErrors.InvalidPassword);
        }

        TokenModel accessTokenModel = _authTokenService.GenerateAccessToken(user);
        TokenModel refreshTokenModel = _authTokenService.GenerateRefreshToken();

        AuthTokenDto authTokenDto = new()
        {
            UserId = user.Id,
            UserEmail = user.Email,
            UserName = user.Name,
            AccessToken = accessTokenModel.Token,
            AccessTokenExpirationAtUtc = accessTokenModel.TokenExpirationAtUtc,
            RefreshToken = refreshTokenModel.Token,
            RefreshTokenExpirationAtUtc = refreshTokenModel.TokenExpirationAtUtc
        };

        RefreshToken refreshToken = new(user.Id, refreshTokenModel.Token, refreshTokenModel.TokenExpirationAtUtc);

        _refreshTokenPersistenceRepository.Insert(refreshToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<AuthTokenDto>.Success(authTokenDto);
    }
}