using Whispr.Application.Core.Dtos.V1;
using Whispr.Application.Core.Services;
using Whispr.Domain.Core.Services;
using Whispr.Domain.Features.Entities.RefreshTokens;
using Whispr.Domain.Features.Entities.Users;
using Whispr.Domain.Features.Models;

namespace Whispr.Application.Features.V1.Auth.Commands.Refresh;

internal sealed class RefreshCommandHandler : ICommandHandler<RefreshCommand, AuthTokenDto>
{
    private readonly IAuthTokenService _authTokenService;
    private readonly ITokenHasherService _tokenHasherService;
    private readonly IRefreshTokenReadOnlyRepository _refreshTokenReadOnlyRepository;
    private readonly IRefreshTokenPersistenceRepository _refreshTokenPersistenceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshCommandHandler(
        IAuthTokenService authTokenService,
        ITokenHasherService tokenHasherService,
        IRefreshTokenReadOnlyRepository refreshTokenReadOnlyRepository,
        IRefreshTokenPersistenceRepository refreshTokenPersistenceRepository,
        IUnitOfWork unitOfWork)
    {
        _authTokenService = authTokenService;
        _tokenHasherService = tokenHasherService;
        _refreshTokenReadOnlyRepository = refreshTokenReadOnlyRepository;
        _refreshTokenPersistenceRepository = refreshTokenPersistenceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AuthTokenDto>> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        RefreshToken? refreshToken = await _refreshTokenReadOnlyRepository.GetByTokenHashAsync(
            _tokenHasherService.Hash(request.RefreshToken),
            cancellationToken);

        if (refreshToken is null)
        {
            return Result<AuthTokenDto>.Failure(RefreshCommandErrors.RefreshTokenNotFound);
        }

        if (refreshToken.IsExpired)
        {
            return Result<AuthTokenDto>.Failure(RefreshCommandErrors.ExpiredRefreshToken);
        }

        User? user = refreshToken.User;

        if (user is null)
        {
            return Result<AuthTokenDto>.Failure(RefreshCommandErrors.InvalidAccessToken);
        }

        TokenModel accessTokenModel = _authTokenService.GenerateAccessToken(user);
        TokenModel refreshTokenModel = _authTokenService.GenerateRefreshToken();
        string refreshTokenHash = _tokenHasherService.Hash(refreshTokenModel.Token);

        RefreshToken newRefreshToken = new(user.Id, TokenHash.Create(refreshTokenHash), refreshTokenModel.TokenExpirationAtUtc);

        refreshToken.Revoke();
        _refreshTokenPersistenceRepository.Update(refreshToken);
        _refreshTokenPersistenceRepository.Insert(newRefreshToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        AuthTokenDto authTokenDto = new()
        {
            AccessToken = accessTokenModel.Token,
            AccessTokenExpirationAtUtc = accessTokenModel.TokenExpirationAtUtc,
            RefreshToken = refreshTokenModel.Token,
            RefreshTokenExpirationAtUtc = refreshTokenModel.TokenExpirationAtUtc
        };

        return Result<AuthTokenDto>.Success(authTokenDto);
    }
}