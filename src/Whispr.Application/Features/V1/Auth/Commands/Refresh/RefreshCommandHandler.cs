using System.Security.Claims;
using Whispr.Application.Core.Interfaces;
using Whispr.Application.Core.Models.V1;
using Whispr.Domain.Core.Interfaces;
using Whispr.Domain.Core.Services;
using Whispr.Domain.Features.Entities.RefreshToken;
using Whispr.Domain.Features.Entities.User;
using Whispr.Domain.Features.Models;
using Whispr.SharedKernel.Results;

namespace Whispr.Application.Features.V1.Auth.Commands.Refresh;

internal sealed class RefreshCommandHandler : ICommandHandler<RefreshCommand, AuthTokenDto>
{
    private readonly IAuthTokenService _authTokenService;
    private readonly IRefreshTokenReadOnlyRepository _refreshTokenReadOnlyRepository;
    private readonly IRefreshTokenPersistenceRepository _refreshTokenPersistenceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshCommandHandler(
        IAuthTokenService authTokenService,
        IRefreshTokenReadOnlyRepository refreshTokenReadOnlyRepository,
        IRefreshTokenPersistenceRepository refreshTokenPersistenceRepository,
        IUnitOfWork unitOfWork)
    {
        _authTokenService = authTokenService;
        _refreshTokenReadOnlyRepository = refreshTokenReadOnlyRepository;
        _refreshTokenPersistenceRepository = refreshTokenPersistenceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AuthTokenDto>> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        RefreshToken? refreshToken = await _refreshTokenReadOnlyRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);

        if (refreshToken is null)
        {
            return Result<AuthTokenDto>.Failure(RefreshCommandErrors.RefreshTokenNotFound);
        }

        if (refreshToken.IsExpired())
        {
            return Result<AuthTokenDto>.Failure(RefreshCommandErrors.ExpiredRefreshToken);
        }

        var userId = TryGetUserIdFromExpiredAccessToken(request.ExpiredAccessToken);

        if (!userId.HasValue)
        {
            return Result<AuthTokenDto>.Failure(RefreshCommandErrors.InvalidAccessToken);
        }

        User? user = refreshToken.User;

        if (user is null)
        {
            return Result<AuthTokenDto>.Failure(RefreshCommandErrors.InvalidAccessToken);
        }

        if (refreshToken.UserId != userId.Value)
        {
            return Result<AuthTokenDto>.Failure(RefreshCommandErrors.InvalidRefreshToken);
        }

        TokenModel accessTokenModel = _authTokenService.GenerateAccessToken(user);
        TokenModel refreshTokenModel = _authTokenService.GenerateRefreshToken();
        RefreshToken newRefreshToken = new(user.Id, refreshTokenModel.Token, refreshTokenModel.TokenExpirationAtUtc);

        _refreshTokenPersistenceRepository.Delete(refreshToken);
        _refreshTokenPersistenceRepository.Insert(newRefreshToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        AuthTokenDto authTokenDto = new()
        {
            UserId = user.Id,
            UserEmail = user.Email,
            UserName = user.Name,
            AccessToken = accessTokenModel.Token,
            AccessTokenExpirationAtUtc = accessTokenModel.TokenExpirationAtUtc,
            RefreshToken = newRefreshToken.Token,
            RefreshTokenExpirationAtUtc = newRefreshToken.ExpirationAtUtc
        };

        return Result<AuthTokenDto>.Success(authTokenDto);
    }

    private Guid? TryGetUserIdFromExpiredAccessToken(string expiredAccessToken)
    {
        ClaimsPrincipal? principal = _authTokenService.GetPrincipalFromAccessToken(expiredAccessToken);

        if (principal is null)
        {
            return null;
        }

        Claim? userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        if (userIdClaim is null)
        {
            return null;
        }

        bool isValidGuid = Guid.TryParse(userIdClaim.Value, out Guid userId);

        return isValidGuid ? userId : null;
    }
}